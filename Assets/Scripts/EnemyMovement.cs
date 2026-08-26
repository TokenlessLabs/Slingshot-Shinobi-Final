using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3.0f;
    private Transform player;
    public bool flipped = false;
    public float separationDistance = 1.0f; // Minimum distance to maintain from other enemies
    public float separationForce = 1.5f; // Force to apply for separation

    private AudioSource audioSource;
    public AudioClip batSound; // Assign the bat sound in the inspector
    public AudioClip zombieGroans; // Assign the zombie sound in the inspector

    private void Awake()
    {
        speed = 3.0f;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        if (gameObject.name.Contains("bat"))
        {
            audioSource.clip = batSound;
        }
        else if (gameObject.name.Contains("Zombie"))
        {
            audioSource.clip = zombieGroans;
        }
    }

    void Update()
    {
        if (GameplayState.IsTerminal)
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            return;
        }

        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 separation = CalculateSeparation();

            if ((player.position - transform.position).magnitude > 0.1)
            {
                Vector3 move = direction * speed * Time.deltaTime + separation * separationForce * Time.deltaTime;
                transform.position += move;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
                if ((direction.x >= 0 && flipped) || (direction.x < 0 && !flipped))
                {
                    Vector3 currentScale = transform.localScale;
                    currentScale.x *= -1;
                    transform.localScale = currentScale;
                    flipped = !flipped;
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }
            }
        }
    }

    Vector3 CalculateSeparation()
    {
        Vector3 separation = Vector3.zero;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, separationDistance);

        foreach (Collider2D collider in colliders)
        {
            if (collider != null && collider.gameObject != this.gameObject && collider.CompareTag("Enemy"))
            {
                Vector3 diff = transform.position - collider.transform.position;
                separation += diff.normalized / diff.magnitude;
            }
        }

        return separation;
    }
}
