using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Drawing;

public class TutorialDash : MonoBehaviour
{
    public Image dashBar; // Reference to the UI Image that acts as the dash bar
    public float cooldown = 1; // Duration for the bar to fill
    public float dashSpeed = 10f; // Speed of the dash
    public float dashDuration = 0.2f; // Duration of the dash
    public float aoeRadius = 1f; // Radius for AoE effect
    public GameObject tearEffectPrefab; // Prefab for the tear effect
    private bool dashed = false;
    public Animator animator;
    public GameObject shurikenPrefab; // Assign the Shuriken prefab in the Inspector

    private Vector2 dashDirection;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private TutorialMovement playerMovement;

    void Start()
    {
        if (dashBar != null)
        {
            dashBar.fillAmount = 0f; // Ensure the dash bar starts empty
        }
        playerMovement = GetComponent<TutorialMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!dashed)
        {
            DetectSwipe();
        }
    }

    void DetectSwipe()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    endTouchPosition = touch.position;
                    Vector2 swipe = endTouchPosition - startTouchPosition;
                    if (swipe.magnitude > 200 && swipe.y < 0 && Vector2.Dot(swipe.normalized,Vector2.down)> Mathf.Cos(20 * Mathf.Deg2Rad))
                    {
                        dashDirection = swipe.normalized;
                        StartCoroutine(DashCoroutine());
                    }
                    break;
            }
        }
    }

    IEnumerator DashCoroutine()
    {
        dashed = true;
        animator.SetBool("Dashing", true);
        float dashTime = 0f;

        // Track the start position
        Vector3 startPosition = transform.position;
        Vector3 dashDirectionVector = new Vector3(dashDirection.x, dashDirection.y, 0);

        while (dashTime < dashDuration)
        {
            Vector3 move = dashDirectionVector * dashSpeed * Time.deltaTime;
            Vector3 newPosition = transform.position + move;

            newPosition.x = Mathf.Clamp(newPosition.x, playerMovement.minX, playerMovement.maxX);
            newPosition.y = Mathf.Clamp(newPosition.y, playerMovement.minY, playerMovement.maxY);

            transform.position = newPosition;

            if (move != Vector3.zero && ((move.x >= 0 && playerMovement.flipped) || (move.x < 0 && !playerMovement.flipped)))
            {
                Vector3 currentScale = transform.localScale;
                currentScale.x *= -1;
                transform.localScale = currentScale;
                playerMovement.flipped = !playerMovement.flipped;
            }

            dashTime += Time.deltaTime;
            yield return null;
        }

        // End position of the dash
        Vector3 endPosition = transform.position;

        if (tearEffectPrefab != null)
        {
            GameObject tearEffect = Instantiate(tearEffectPrefab, endPosition, Quaternion.identity);
            SpriteRenderer tearSpriteRenderer = tearEffect.GetComponent<SpriteRenderer>();

            if (tearSpriteRenderer != null)
            {
                // Calculate the length of the dash and adjust it as needed
                float length = Vector3.Distance(startPosition, endPosition);

                // Set the size of the tear effect
                Vector3 newSize = new Vector3(length / tearSpriteRenderer.size.x, (length / tearSpriteRenderer.size.x) / 2 > 15 ? 15 : (length / tearSpriteRenderer.size.x) / 2, 1); // Adjust for sprite size
                tearEffect.transform.localScale = newSize;

                // Adjust the position so the end aligns with the end position
                Vector3 direction = (endPosition - startPosition).normalized;
                Vector3 offset = direction * (length / 2f);
                tearEffect.transform.position = startPosition + offset;

                // Set the tear effect’s direction
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                tearEffect.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }

            Destroy(tearEffect, 5f);
        }
        ApplyAoEEffect();
        animator.SetBool("Dashing", false);
    }

    void ApplyAoEEffect()
    {
        // Find all colliders within the AoE radius
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
                Collider2D collider = enemy.GetComponent<Collider2D>();
                // Spawn a shuriken at the enemy's position
                StartCoroutine(SpawnShuriken(collider));
        }
    }

    IEnumerator SpawnShuriken(Collider2D enemy)
    {
        Vector3 enemyPosition = enemy.transform.position; // Store the enemy's position

        // Disable enemy's scripts and set the animation trigger before destroying the enemy
        Animator animator = enemy.GetComponent<Animator>();
        animator.SetTrigger("Dead");
        MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false;
        }
        // Notify the GameManager that an enemy has been killed
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null && Time.timeScale != 0)
        {
            gameManager.EnemyKilled();
        }
        // Wait for the animation to play
        yield return new WaitForSeconds(1.5f);

        // Destroy the enemy game object
        Destroy(enemy.gameObject);

        // Instantiate the shuriken at the stored position
       Instantiate(shurikenPrefab, enemyPosition, Quaternion.identity);
    }
}
