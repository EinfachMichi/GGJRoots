using System;
using Enemy;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;
    [SerializeField, Range(0.01f, 1f)] private float groundAcceleration;
    [SerializeField, Range(0.01f, 1f)] private float groundStoppingAcceleration;
    [SerializeField, Range(0.01f, 1f)] private float airAcceleration;
    [SerializeField, Range(0.01f, 1f)] private float airStoppingAcceleration;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float checkRadius;
    [SerializeField] private BoxCollider2D collider;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float horizontalAttackpointPosition;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask enemyLayer;

    private const float skinWidth = 0.15f;
    private const float gravity = -9.81f;
    
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Camera cam;
    private float horizontalVelocity, verticalVelocity;
    private int mouseDirection = 1;
    private int direction;
    private bool isGrounded;
    private float attackCooldownCounter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        float xOffset = transform.position.x - mousePos.x;
        if (xOffset < 0) mouseDirection = 1;
        else mouseDirection = -1;

        if (attackCooldownCounter <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attackCooldownCounter = attackCooldown;
                anim.SetBool("Attacking", true);
                anim.SetTrigger("Attack");
            }
        }
        attackCooldownCounter -= Time.deltaTime;

        GroundCheck();
        HorizontalMovement(horizontalInput);
        VerticalMovement(jump);
        Flip();
    }

    #region Attack

    private void ResetAttack()
    {
        anim.SetBool("Attacking", false);
    }
    
    private void Attack()
    {
        Vector2 origin;
        if (mouseDirection == 1) origin = new Vector2(transform.position.x + horizontalAttackpointPosition, transform.position.y);
        else origin = new Vector2(transform.position.x - horizontalAttackpointPosition, transform.position.y);

        Collider2D[] cols = Physics2D.OverlapCircleAll(
            origin,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D col in cols)
        {
            col.GetComponent<EnemyManager>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 origin;
        if (mouseDirection == 1) origin = new Vector2(transform.position.x + horizontalAttackpointPosition, transform.position.y + collider.bounds.extents.y);
        else origin = new Vector2(transform.position.x - horizontalAttackpointPosition, transform.position.y + collider.bounds.extents.y);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin, attackRadius);
    }

    #endregion
    
    #region Movement

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalVelocity, verticalVelocity);
    }

    private void HorizontalMovement(float horizontalInput)
    {
        float acceleration;
        if (horizontalInput != 0)
        {
            if (isGrounded) acceleration = groundAcceleration;
            else acceleration = airAcceleration;
            direction = (int) horizontalInput;
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
        
        anim.SetFloat("Speed", Mathf.Abs(horizontalVelocity));
    }

    private void VerticalMovement(bool jump)
    {
        if (isGrounded)
        {
            if (rb.velocity.y == 0 && verticalVelocity < 0) verticalVelocity = 0;
            
            if (jump)
            {
                anim.SetTrigger("Jump");
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            verticalVelocity += gravity * gravityScale * Time.deltaTime;
        }
        
        anim.SetFloat("VerticalVelocity", rb.velocity.y);
    }

    private void Flip()
    {
        sr.flipX = mouseDirection == -1;
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

    #endregion
}
