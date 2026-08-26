using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMovement: MonoBehaviour
{
    public float speed = 5.0f;
    public Joystick joystick;
    public Animator animator;

    // Boundary variables (set these values based on your Tilemap bounds)
    public float minX = -1000f;
    public float maxX = 1000f;
    public float minY = -1000f;
    public float maxY = 1000f;
    public bool flipped = false;

    private bool isJoystickLocked = true; // Initially lock the joystick

    void Start()
    {
        if (MapBoundarySettings.TryGetWalkableBounds(out Bounds walkableBounds))
        {
            float playerHalfWidth = GetComponent<Collider2D>()?.bounds.extents.x ?? 0f;
            float playerHalfHeight = GetComponent<Collider2D>()?.bounds.extents.y ?? 0f;
            minX = walkableBounds.min.x + playerHalfWidth;
            maxX = walkableBounds.max.x - playerHalfWidth;
            minY = walkableBounds.min.y + playerHalfHeight;
            maxY = walkableBounds.max.y - playerHalfHeight;
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

        if (isJoystickLocked)
        {
            animator.SetFloat("Speed", 0);
            return; // Do not process movement if joystick is locked
        }

        Vector2 input = joystick.InputVector;
        Vector3 move = new Vector3(input.x, input.y, 0);
        Vector3 newPosition = transform.position + move * speed * Time.deltaTime;

        // Clamp the new position within the boundaries
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        // Update the transform's position
        transform.position = newPosition;

        // Update animator with movement speed (using Mathf.Abs to ensure it's positive)
        animator.SetFloat("Speed", Mathf.Abs(move.magnitude));

        // Update the player's facing direction
        if (move != Vector3.zero && ((move.x >= 0 && flipped) || (move.x < 0 && !flipped)))
        {
            Vector3 currentScale = transform.localScale;
            currentScale.x *= -1; // Invert the X scale to flip the sprite
            transform.localScale = currentScale;
            flipped = !flipped;
        }
    }

    public void SetJoystickLock(bool locked)
    {
        isJoystickLocked = locked;
    }
}
