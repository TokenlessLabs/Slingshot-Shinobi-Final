using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    public GameObject gameOverPanel;
    private Animator animator;
    public GameObject mainCamera;
    public GameObject health;
    public GameObject dashbar;
    public float zoomDuration = 2f;
    public float zoomSize = 5f;
    public AudioSource damageAudioSource;
    public AudioClip damageSound;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        animator = GetComponent<Animator>();
        if (damageAudioSource == null)
        {
            Debug.LogError("Damage AudioSource not assigned!");
        }
    }

    private void Update()
    {
        healthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0)
        {
            Debug.LogWarning("Damage amount must be positive.");
            return;
        }

        animator.SetTrigger("Hit");
        currentHealth -= amount;

        // Play the damage sound
        if (damageAudioSource != null && damageSound != null)
        {
            damageAudioSource.clip = damageSound;

            if (!damageAudioSource.isPlaying) 
            {
                damageAudioSource.Stop(); 
                damageAudioSource.Play();
                Debug.Log("Playing damage sound");
            }
            else
            {
                Debug.LogWarning("Damage sound is already playing.");
            }
        }
        else
        {
            Debug.LogWarning("DamageAudioSource or DamageSound not assigned!");
        }

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        DisableAllEnemyScripts();
        SpawnManager spawner = FindObjectOfType<SpawnManager>();
        if (spawner != null)
        {
            spawner.StopAllCoroutines();
            spawner.enabled = false;
        }
        BossSpawnManager BossSpawner = FindObjectOfType<BossSpawnManager>();
        if (BossSpawner != null)
        {
            BossSpawner.StopAllCoroutines();
            BossSpawner.enabled = false;
        }
        health.SetActive(false);
        dashbar.SetActive(false);
        PlayerMovementWithJoystick movement = GetComponent<PlayerMovementWithJoystick>();
        movement.enabled = false;
        Dash dash = GetComponent<Dash>();
        dash.enabled = false;
        PlayerShooting shooting = GetComponent<PlayerShooting>();
        shooting.enabled = false;
        animator.SetTrigger("Died");
        StartCoroutine(ZoomCameraAndEndGame());
    }

    private IEnumerator ZoomCameraAndEndGame()
    {
        Camera cam = mainCamera.GetComponent<Camera>();
        float startSize = cam.orthographicSize;
        float targetSize = 5f;
        float duration = 2f; 
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            cam.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cam.orthographicSize = targetSize; 
        yield return new WaitForSeconds(2); 

        // Call the game over manager
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void DisableAllEnemyScripts()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Animator animator = enemy.GetComponent<Animator>();
            animator.enabled = false;
            MonoBehaviour[] scripts = enemy.GetComponents<MonoBehaviour>();

            foreach (var script in scripts)
            {
                script.enabled = false;
            }
        }
    }
}