using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Shuriken : MonoBehaviour
{
    public float collectionRadius = 1.0f; // Set the collection radius
    private float moveSpeed = 20f; // Speed at which the shuriken moves towards the player
    private Transform playerTransform;
    private bool isAbsorbing = false;
    private bool hasBeenCollected = false;

    void Start()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        collider.isTrigger = true;
        collider.radius = collectionRadius;
    }

    void Update()
    {
        if (isAbsorbing && playerTransform != null)
        {
            // Move the shuriken towards the player
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

            // Check if the shuriken has reached the player
            if (Vector3.Distance(transform.position, playerTransform.position) < 0.1f)
            {
                isAbsorbing = false;
                transform.position = playerTransform.position; // Ensure the shuriken is at the player's position

                if (!hasBeenCollected)
                {
                    PlayerCollecting playerCollecting = playerTransform.GetComponent<PlayerCollecting>();
                    playerCollecting.CollectShuriken();
                    hasBeenCollected = true;
                }

                Destroy(gameObject); 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasBeenCollected)
        {
            playerTransform = other.transform;
            isAbsorbing = true; 
        }
    }

    public void ForcedAbsorb(Collider2D other)
    {
        playerTransform = other.transform;
        isAbsorbing = true; 
    }
}