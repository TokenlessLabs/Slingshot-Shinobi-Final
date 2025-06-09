using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private BossHealthBarsScript healthBar;
 
    void Start()
    {
        healthBar = FindObjectOfType<BossHealthBarsScript>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    void Die()
    {
        Destroy(gameObject);
        healthBar.GameComplete();       
    }
}