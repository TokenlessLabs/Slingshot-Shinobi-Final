using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Drawing;

public class Dash : MonoBehaviour
{
    public Image dashBar; // Reference to the UI Image that acts as the dash bar
    public float cooldown = 1; // Duration for the bar to fill
    public float dashSpeed = 10f; // Speed of the dash
    public float dashDuration = 0.2f; // Duration of the dash
    public float aoeRadius = 1f; // Radius for AoE effect
    public GameObject tearEffectPrefab; // Prefab for the tear effect
    private float fillTimer = 0f;
    private bool ready = false;
    public bool isDashing = false;
    public Animator animator;
    public GameObject shurikenPrefab; 

    private Vector2 dashDirection;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private PlayerMovementWithJoystick playerMovement;
    private Joystick joystick;
    private PowerupPanel powerupPanel;
    private bool bossHit = false;

    void Start()
    {
        if (dashBar != null)
        {
            dashBar.fillAmount = 0f; 
        }
        playerMovement = GetComponent<PlayerMovementWithJoystick>();
        joystick = FindObjectOfType<Joystick>(); 
        animator = GetComponent<Animator>();
        powerupPanel = GetComponent<PowerupPanel>();
    }

    void Update()
    {
        if (GameplayState.IsTerminal || GameplayState.IsPaused || GameplayState.IsPowerupOpen)
        {
            return;
        }

        if (dashBar != null && !ready)
        {
            fillTimer += Time.deltaTime;
            dashBar.fillAmount = cooldown>0?fillTimer / cooldown:1;
            if (fillTimer >= cooldown)
            {
                fillTimer = 0f;
                ready = true;
            }
        }

        if (ready && !isDashing)
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
                    if (swipe.magnitude > 200&& joystick.InputVector==Vector2.zero)
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
        ready = false;
        isDashing = true;
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
            ApplyAoEEffect();
            yield return null;
        }
        Vector3 endPosition = transform.position;

        if (tearEffectPrefab != null)
        {
            GameObject tearEffect = Instantiate(tearEffectPrefab, endPosition, Quaternion.identity);
            SpriteRenderer tearSpriteRenderer = tearEffect.GetComponent<SpriteRenderer>();

            if (tearSpriteRenderer != null)
            {
                float length = Vector3.Distance(startPosition, endPosition);
                Vector3 newSize = new Vector3(length / tearSpriteRenderer.size.x, (length / tearSpriteRenderer.size.x) / 2 > 15 ? 15 : (length / tearSpriteRenderer.size.x) / 2, 1); // Adjust for sprite size
                tearEffect.transform.localScale = newSize;
                Vector3 direction = (endPosition - startPosition).normalized;
                Vector3 offset = direction * (length / 2f);
                tearEffect.transform.position = startPosition + offset;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                tearEffect.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            }

            Destroy(tearEffect, 5f);
        }
        if (bossHit)
        {
            bossHit = false;
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BossHealth>().TakeDamage(5);
            GameObject.FindGameObjectWithTag("Boss").GetComponent<Animator>().SetTrigger("Hit");
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BossEnemyAttack>().isAttacking = false;
        }
        isDashing = false;
        animator.SetBool("Dashing", false);
    }

    void ApplyAoEEffect()
    {
        // Find all colliders within the AoE radius
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, aoeRadius);

        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
                if (movement == null || !movement.enabled)
                {
                    continue; 
                }
                StartCoroutine(SpawnShuriken(enemy));
            }
            else if (enemy.CompareTag("Boss"))
            {
                bossHit = true;
            }
        }
    }

    IEnumerator SpawnShuriken(Collider2D enemy)
    {
        Vector3 enemyPosition = enemy.transform.position; 
        Animator animator = enemy.GetComponent<Animator>();
        animator.SetTrigger("Dead");
        MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false;
        }
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null && Time.timeScale != 0)
        {
            gameManager.EnemyKilled();
        }
       
        yield return new WaitForSeconds(1.5f);
        Destroy(enemy.gameObject);
        GameObject shuriken = Instantiate(shurikenPrefab, enemyPosition, Quaternion.identity);
        Shuriken shurikenScript = shuriken.GetComponent<Shuriken>();
        Collider2D collider = GetComponent<Collider2D>();
        shurikenScript.ForcedAbsorb(collider);
    }


    public void ResetCooldown(float duration)
    {
        StartCoroutine(ResetCooldownAfterDelay(duration));
    }

    IEnumerator ResetCooldownAfterDelay(float duration)
    {
        yield return new WaitForSeconds(0.1f);
        float originalCooldown = cooldown;
        cooldown = 0f;
        yield return new WaitForSeconds(duration);
        cooldown = originalCooldown;
    }

}
