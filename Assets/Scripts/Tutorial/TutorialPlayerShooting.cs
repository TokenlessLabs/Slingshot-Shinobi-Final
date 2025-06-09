using UnityEngine;

public class TutorialPlayerShooting : MonoBehaviour
{
    public GameObject pebblePrefab;
    public Transform shootPoint;
    public float shootForce = 10f;
    public float shootInterval = 0.5f;
    public string enemyTag = "Enemy";

    private float shootTimer;
    private bool canShoot = false; // Added

    void Start()
    {
        if (pebblePrefab == null) Debug.LogError("Pebble prefab is not assigned in the Inspector!");
        if (shootPoint == null) Debug.LogError("Shoot point is not assigned in the Inspector!");
    }

    void Update()
    {
        if (!canShoot) return; // Added

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    public void SetShootingEnabled(bool enabled) // Added
    {
        canShoot = enabled;
    }

    void Shoot()
    {
        if (pebblePrefab == null || shootPoint == null) return;

        GameObject closestEnemy = FindClosestEnemy();
        if (closestEnemy == null) return;

        Vector2 shootingDirection = (closestEnemy.transform.position - shootPoint.position).normalized;

        GameObject pebble = Instantiate(pebblePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = pebble.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = shootingDirection * shootForce;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(shootPoint.position, enemy.transform.position);
            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
}
