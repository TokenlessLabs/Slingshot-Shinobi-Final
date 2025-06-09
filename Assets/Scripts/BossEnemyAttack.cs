using System.Collections;
using UnityEngine;

public class BossEnemyAttack : MonoBehaviour
{
    public int damageAmount = 40; // Damage amount 
    public float attackInterval = 1.5f; // Time interval between consecutive attacks
    private Animator animator; //Animator component
    private float attackTimer;
    public bool isAttacking = false;

    void Start()
    {

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= attackInterval)
            {
                StartCoroutine(PerformAttack());
                attackTimer = 0f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("swhoompl");
            isAttacking = true;
            attackTimer = 2f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            isAttacking = false;
            animator.ResetTrigger("Cleave");
        }
    }

    IEnumerator PerformAttack()
    {
        animator.SetTrigger("Cleave");
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        yield return new WaitForSeconds(2f);
        if (playerHealth != null && isAttacking)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }
}
