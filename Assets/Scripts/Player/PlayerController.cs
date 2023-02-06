using System;
using System.Collections;
using Enemy;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float dashDuration;
    [SerializeField] private float waitAfterDash;
    [SerializeField, Range(0.01f, 1f)] private float groundAcceleration;
    [SerializeField, Range(0.01f, 1f)] private float groundStoppingAcceleration;
    [SerializeField, Range(0.01f, 1f)] private float airAcceleration;
    [SerializeField, Range(0.01f, 1f)] private float airStoppingAcceleration;
    [SerializeField] private LayerMask collisionLayer;
    [SerializeField] private float checkRadius;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private GameObject gameOverScreen;

    [Header("Attack")]
    [SerializeField] private int damage;
    [SerializeField] private float shootDamage;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float horizontalAttackpointPosition;
    [SerializeField] private float attackRadius;
    [SerializeField] private float shootCooldown;
    [SerializeField] private float shootForce;
    [SerializeField] private Vector2 knockbackDirection;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform shootAnchor;
    [SerializeField] private Transform partenTrans;
    [SerializeField] private GameObject floatingText;
    [SerializeField] private Transform floatingSpawnPoint;

    private const float skinWidth = 0.15f;
    private const float gravity = -9.81f;
    
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private Camera cam;
    private Vector2 mousePos;
    [HideInInspector] public float horizontalVelocity;
    private float verticalVelocity;
    private int mouseDirection = 1;
    private int direction = 1;
    private bool isGrounded;
    private float attackCooldownCounter;
    private float shootCooldownCounter;
    private bool charge;
    private float speed;
    private float dashTimeCounter;
    private bool isDashing, canDash = true;
    private bool isStunned;
    [HideInInspector] public bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        cam = Camera.main;
    }

    private void Start()
    {
        speed = moveSpeed;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        bool jump = Input.GetKeyDown(KeyCode.Space);
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        Flip();
        
        float xOffset = transform.position.x - mousePos.x;
        if (xOffset < 0) mouseDirection = 1;
        else mouseDirection = -1;

        attackCooldownCounter -= Time.deltaTime;
        if (attackCooldownCounter <= 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                attackCooldownCounter = attackCooldown;
                anim.SetBool("Attacking", true);
                anim.SetTrigger("Attack");
            }
        }
        
        if (canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
        anim.SetBool("IsDashing", isDashing);
        
        shootCooldownCounter -= Time.deltaTime;
        if (Input.GetMouseButton(1) && shootCooldownCounter <= 0)
        {
            sr.flipX = mouseDirection == -1;
            anim.SetBool("Charge", true);
            speed = moveSpeed / 2;
            charge = true;
        }
        else
        {
            if (charge)
            {
                ShootProjectile();
                shootCooldownCounter = shootCooldown;
            }
            charge = false;
            anim.SetBool("Charge", false);
            speed = moveSpeed;
        }

        GroundCheck();
        HorizontalMovement(horizontalInput);
        VerticalMovement(jump);
    }

    private IEnumerator Dash()
    {
        horizontalVelocity = dashForce * 2f * direction;
        isDashing = true;
        canDash = false;
        verticalVelocity = 0;
        anim.SetTrigger("Dash");
        yield return new WaitForSeconds(dashDuration);
        horizontalVelocity = 0;
        isDashing = false;
        StartCoroutine(StunTime());
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator StunTime()
    {
        isStunned = true;
        yield return new WaitForSeconds(waitAfterDash);
        isStunned = false;
    }
    
    #region Attack

    private void ResetAttack()
    {
        anim.SetBool("Attacking", false);
    }

    private void ShootProjectile()
    {
        if(mouseDirection > 0) shootAnchor.localScale = new Vector3(1, 1, 1);
        else shootAnchor.localScale = new Vector3(-1, 1, 1);

        Vector2 dir = (mousePos - (Vector2) transform.position).normalized;
        GameObject proj = Instantiate(projectile, shootPoint.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().AddForce(dir * shootForce, ForceMode2D.Impulse);
        proj.GetComponent<Projectile>().damage = (int) shootDamage;
        proj.GetComponent<Projectile>().knockbackDirection = new Vector2(knockbackDirection.x * mouseDirection / 2, knockbackDirection.y);
    }
    
    private void MeleeAttack()
    {
        Vector2 origin;
        if (direction == 1) origin = new Vector2(transform.position.x + horizontalAttackpointPosition, transform.position.y);
        else origin = new Vector2(transform.position.x - horizontalAttackpointPosition, transform.position.y);

        Collider2D[] cols = Physics2D.OverlapCircleAll(
            origin,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D col in cols)
        {
            col.GetComponent<EnemyManager>().TakeDamage(damage, new Vector2(knockbackDirection.x * direction, knockbackDirection.y));
        }
    }

    private void OnDrawGizmos()
    {
        Vector2 origin;
        if (direction == 1) origin = new Vector2(transform.position.x + horizontalAttackpointPosition, transform.position.y + collider.bounds.extents.y);
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
        if (isDashing || isStunned || isDead) return;
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
        if (isDashing || isStunned) return;
        if (isGrounded)
        {
            if (rb.velocity.y == 0 && verticalVelocity < 0) verticalVelocity = 0;
            
            if (jump && !isDead)
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
        if(!charge) sr.flipX = direction == -1;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Border") || col.CompareTag("Root") || col.CompareTag("Snail") || col.CompareTag("Spider") || col.CompareTag("SpiderWeb") || col.CompareTag("Projectile")) return;

        string text = "";
        if (col.CompareTag("Strength"))
        {
            damage += 1;
            shootDamage += 0.5f;
            Destroy(col.gameObject);
            text = "+STRENGTH";
            if (damage >= 6)
            {
                damage = 6;
            }
            AudioManager.instance.Play("PowerUp", AudioManager.instance.effectSounds);
        }
        else if (col.CompareTag("Speed"))
        {
            moveSpeed += 1;
            Destroy(col.gameObject);
            text = "+SPEED";
            if (moveSpeed >= 10)
            {
                moveSpeed = 10;
            }
            AudioManager.instance.Play("PowerUp", AudioManager.instance.effectSounds);
        }
        else if (col.CompareTag("Jump"))
        {
            jumpForce += 5;
            Destroy(col.gameObject);
            text = "+JUMP";
            if (jumpForce >= 30)
            {
                jumpForce = 30;
            }
            AudioManager.instance.Play("PowerUp", AudioManager.instance.effectSounds);
        }
        else if (col.CompareTag("Heal"))
        {
            FindObjectOfType<RootManager>().Heal(2);
            Destroy(col.gameObject);
            AudioManager.instance.Play("Heal", AudioManager.instance.effectSounds);
            text = "+HEAL";
        }

        GameObject ft = Instantiate(floatingText, floatingSpawnPoint.position, Quaternion.identity);
        ft.GetComponent<FloatingText>().statText.text = text;
        ft.transform.parent = partenTrans;
    }

    private void GameOver()
    {
        gameOverScreen.SetActive(true);
        AudioManager.instance.Play("GameOver", AudioManager.instance.effectSounds);
        AudioManager.instance.Stop("Theme", AudioManager.instance.music);
        Destroy(gameObject);
    }
}
