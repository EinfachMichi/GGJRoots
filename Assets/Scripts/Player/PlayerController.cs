using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;
    [SerializeField, Range(0.01f, 1f)] private float groundAcceleration;
    [SerializeField, Range(0.01f, 1f)] private float groundStoppingAcceleration;
    [SerializeField, Range(0.01f, 1f)] private float airAcceleration;
    [SerializeField, Range(0.01f, 1f)] private float airStoppingAcceleration;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float checkRadius;

    private const float skinWidth = 0.15f;
    private const float gravity = -9.81f;
    
    private BoxCollider2D collider;
    private Rigidbody2D rb;
    private float horizontalVelocity, verticalVelocity;
    private int direction;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        
        GroundCheck();
        HorizontalMovement(horizontalInput);
        VerticalMovement(jump);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    private void HorizontalMovement(float horizontalInput)
    {
        float acceleration;
        if (horizontalInput != 0)
        {
            direction = (int) horizontalInput;

            if (isGrounded) acceleration = groundAcceleration;
            else acceleration = airAcceleration;
                
            horizontalVelocity += speed / acceleration * direction * Time.deltaTime;
            if (horizontalVelocity >= speed) horizontalVelocity = speed;
            else if (horizontalVelocity <= -speed) horizontalVelocity = -speed;
        }
        else
        {
            if (isGrounded) acceleration = groundStoppingAcceleration;
            else acceleration = airStoppingAcceleration;
            
            horizontalVelocity += speed / acceleration * direction * Time.deltaTime * -1f;
            if ((direction > 0 && horizontalVelocity < 0) ||
                direction < 0 && horizontalVelocity > 0)
            {
                horizontalVelocity = 0;
            }
        }
    }

    private void VerticalMovement(bool jump)
    {
        if (isGrounded)
        {
            if (rb.velocity.y == 0 && verticalVelocity < 0) verticalVelocity = 0;
            
            if (jump)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity += gravity * gravityScale * Time.deltaTime;
        }
    }

    private void GroundCheck()
    {
        Bounds bounds = collider.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.center.y - bounds.extents.y);
        Vector2 size = new Vector2(bounds.size.x - skinWidth, checkRadius);

        isGrounded = Physics2D.OverlapCapsule(
            origin,
            size, 
            CapsuleDirection2D.Horizontal,
            0f,
            collisionLayer
            );
    }
}
