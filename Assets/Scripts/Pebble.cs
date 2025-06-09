using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pebble : MonoBehaviour
{
    public GameObject shurikenPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            HandleEnemyHit(other);
        }
        else if(other.CompareTag("Boss"))
        {
            other.GetComponent<BossHealth>().TakeDamage(1);
            Destroy(gameObject);
        }
    }

    void HandleEnemyHit(Collider2D enemyCollider)
    {
        EnemyMovement movement = enemyCollider.GetComponent<EnemyMovement>();
        if (movement == null || !movement.enabled)
        {
            return;
        }
        Instantiate(shurikenPrefab, enemyCollider.transform.position, Quaternion.identity);

        Animator animator = enemyCollider.GetComponent<Animator>();
        animator.SetTrigger("Shot");

        MonoBehaviour[] scripts = enemyCollider.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = false;
        }

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null && Time.timeScale != 0)
        {
            gameManager.EnemyKilled();
        }

        Destroy(enemyCollider.gameObject, 1);
        Destroy(gameObject);
    }
}
