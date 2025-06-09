using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject pebblePrefab;  // The pebble prefab
    public Transform shootPoint;     // The point from where the pebble will be shot
    public float shootForce = 10f;   // The force with which the pebble will be shot
    public float shootInterval = 0.5f; // Time interval between shots
    public string[] targetTags = {"Enemy", "Boss" };  
    private float shootTimer;

    void Start()
    {
        if (pebblePrefab == null)
        {
            Debug.LogError("Pebble prefab is not assigned in the Inspector!");
        }
        if (shootPoint == null)
        {
            Debug.LogError("Shoot point is not assigned in the Inspector!");
        }
    }

    void Update()
    {
        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            StartCoroutine(ShootWithDelay());
            shootTimer = 0f;
        }
    }

    IEnumerator ShootWithDelay()
    {
        GameObject closestEnemy = FindClosestTarget();
        if (closestEnemy == null) yield break; 
        Animator animator = shootPoint.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Shoot");
        }
        else
        {
            Debug.LogError("No Animator found on the shootPoint!");
        }
        yield return new WaitForSeconds(0.5f);
        Vector2 shootingDirection = (closestEnemy.transform.position - shootPoint.position).normalized;
        GameObject pebble = Instantiate(pebblePrefab, shootPoint.position, shootPoint.rotation);
        if (pebble == null)
        {
            Debug.LogError("Failed to instantiate pebble!");
            yield break;
        }
        Rigidbody2D rb = pebble.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootingDirection * shootForce;
        }
        else
        {
            Debug.LogError("No Rigidbody2D found on the pebble prefab");
        }
    }

    GameObject FindClosestTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (string tag in targetTags)
        {
            GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject target in targets)
            {
                if (target == null)
                {
                    continue; 
                }

                EnemyMovement movement = target.GetComponent<EnemyMovement>();
                if (tag == "Enemy" && movement == null)
                {
                    continue; 
                }

                if (tag == "Enemy" && !movement.enabled)
                {
                    continue;
                }

                float distanceToTarget = Vector2.Distance(shootPoint.position, target.transform.position);
                if (distanceToTarget < closestDistance)
                {
                    closestDistance = distanceToTarget;
                    closestTarget = target;
                }
            }
        }

        return closestTarget;
    }
}
