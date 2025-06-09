using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyMovement : MonoBehaviour
{
    public float speed = 3.0f; // Movement speed of the boss
    private Transform player; // Reference to the player's transform
    public bool flipped = false; // Flag to track sprite flipping
    private Animator animator; // Reference to the Animator component

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing!");
        }
    }

    void Update()
    {
        if (player != null)
        { 
            Vector3 direction = (player.position - transform.position).normalized;
            if ((player.position - transform.position).magnitude > 0.1)
            {          
                Vector3 move = direction * speed * Time.deltaTime;               
                transform.position += move;               
                if ((direction.x >= 0 && flipped) || (direction.x < 0 && !flipped))
                {
                    Vector3 currentScale = transform.localScale;
                    currentScale.x *= -1; 
                    transform.localScale = currentScale;
                    flipped = !flipped;
                }              
                if (move.magnitude > 0)
                {
                    animator.SetBool("IsWalking", true);
                }
                else
                {
                    animator.SetBool("IsWalking", false);
                }
            }
        }
    }
}