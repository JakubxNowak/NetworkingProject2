using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float jumpForce = 12f;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;

    private void Awake()
    {
        // Cache Rigidbody2D reference for movement
        rb = GetComponent<Rigidbody2D>();

        // Prevent the player from flipping over
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Only the owner client can control their player
        if (!IsOwner) return;

        float moveInput = Input.GetAxisRaw("Horizontal");

        // Horizontal movement
        rb.linearVelocity = new Vector2(
            moveInput * moveSpeed,
            rb.linearVelocity.y
        );

        // Jump if the jump button is pressed and the player is grounded
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            // Reset vertical velocity so jumps feel consistent
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            // Apply an upward impulse
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private bool IsGrounded()
    {
        // OverlapCircleAll returns all colliders in the circle
        // filters out our own collider so players can infite jump
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        foreach (var hit in hits)
        {
            if (hit == null) continue;

            // Ignore players own colliders
            if (hit.transform == transform) continue;
            if (hit.transform.IsChildOf(transform)) continue;

            // Player is grounded if it hits anything else on the GroundLayer
            return true;
        }

        return false;
    }

}
