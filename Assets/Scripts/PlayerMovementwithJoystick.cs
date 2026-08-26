using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWithJoystick : MonoBehaviour
{
    public float speed = 5.0f;
    public Joystick joystick;
    public Animator animator;
    public float minX = -1000f;
    public float maxX = 1000f;
    public float minY = -1000f;
    public float maxY = 1000f;
    public bool flipped = false;

    void Start()
    {
        if (LevelBoundary.TryGetBounds("Level 1", out Bounds mapBounds)
            || LevelBoundary.TryGetBounds("Level 2", out mapBounds)
            || LevelBoundary.TryGetBounds("Level 3", out mapBounds)
            || LevelBoundary.TryGetBounds("Level 4", out mapBounds)
            || LevelBoundary.TryGetBounds("Level 5", out mapBounds))
        {
            float playerHalfWidth = GetComponent<Collider2D>()?.bounds.extents.x ?? 0f;
            float playerHalfHeight = GetComponent<Collider2D>()?.bounds.extents.y ?? 0f;
            minX = mapBounds.min.x + playerHalfWidth;
            maxX = mapBounds.max.x - playerHalfWidth;
            minY = mapBounds.min.y + playerHalfHeight;
            maxY = mapBounds.max.y - playerHalfHeight;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    void Update()
    {
        if (GameplayState.IsTerminal || GameplayState.IsPaused || GameplayState.IsPowerupOpen)
        {
            animator.SetFloat("Speed", 0f);
            return;
        }

        Vector2 input = joystick.InputVector;
        Vector3 move = new Vector3(input.x, input.y, 0);
        Vector3 newPosition = transform.position + move * speed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
        animator.SetFloat("Speed", Mathf.Abs(move.magnitude));
        if (move != Vector3.zero && ((move.x >= 0 && flipped) || (move.x < 0 && !flipped)))
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x *= -1; 
            transform.localScale = currentScale;
            flipped = !flipped;
        }
    }
}