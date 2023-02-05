using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] public int health;
        [SerializeField] private float speed;
        [SerializeField] private float stoppingDistance;
        [SerializeField] private float attackCooldown;
        [SerializeField] private float knockbackTime;
        [SerializeField] private float destroyTime;
        [SerializeField] private float stunTime;
        [Space] 
        [SerializeField] private bool knockback;
        [SerializeField] private float horizontalAttackpointPosition;
        [SerializeField] private float attackRadius;
        [SerializeField] private LayerMask rootLayer;

        private Rigidbody2D rb;
        private Animator anim;
        private PhysicsMaterial2D rbMat;
        private SpriteRenderer sr;
        [HideInInspector] public int direction;
        private float horizontalVelocity;
        private float attackCooldownCounter;
        private float knockbackTimeCounter;
        [HideInInspector] public bool isDying;
        private bool playDeathAnim;
        private bool isStunned;
        
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            sr = GetComponent<SpriteRenderer>();
            rbMat = rb.sharedMaterial;
        }

        private void Start()
        {
            if (transform.position.x > 0) direction = -1;
            else if (transform.position.x < 0) direction = 1;
            transform.localScale = new Vector3( direction * -1f, 1f, 1f);
            
            horizontalVelocity = direction * speed;
        }

        private void FixedUpdate()
        {
            if (knockbackTimeCounter <= 0)
            {
                if (isDying && !playDeathAnim)
                {
                    playDeathAnim = true;
                    anim.SetTrigger("Death");
                }
                anim.SetBool("KnockbackOver", true);
                rb.sharedMaterial = null;
                if(isDying)
                {
                    horizontalVelocity = 0;
                }
                rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
            }
        }

        private void Update()
        {
            knockbackTimeCounter -= Time.deltaTime;
            if (knockbackTimeCounter <= 0 && !isDying && !isStunned)
            {
                if (direction == 1)
                {
                    if (transform.position.x >= -stoppingDistance)
                    {
                        attackCooldownCounter -= Time.deltaTime;
                        if (attackCooldownCounter <= 0)
                        {
                            Attack();
                        }
                        horizontalVelocity = 0;
                    }
                    else
                    {
                        horizontalVelocity = direction * speed;
                    }
                }
                else if(direction == -1)
                {
                    if (transform.position.x <= stoppingDistance)
                    {
                        attackCooldownCounter -= Time.deltaTime;
                        if (attackCooldownCounter <= 0)
                        {
                            Attack();
                        }
                        horizontalVelocity = 0;
                    }
                    else
                    {
                        horizontalVelocity = direction * speed;
                    }
                }
                anim.SetFloat("Speed", Mathf.Abs(horizontalVelocity));
            }
        }

        public void TakeDamage(int damage, Vector2 knockbackDir)
        {
            if (isDying) return;
            health -= damage;
            Knockback(knockbackDir);
        }

        private void Knockback(Vector2 knockbackDir)
        {
            if (knockback)
            {
                anim.SetTrigger("Knockback");
                anim.SetBool("KnockbackOver", false);
                rb.sharedMaterial = rbMat;
                rb.velocity = new Vector2(knockbackDir.x, knockbackDir.y);
                knockbackTimeCounter = knockbackTime;
            }
            else
            {
                anim.SetTrigger("Hit");
            }
        }

        private void StartStun()
        {
            StartCoroutine(Stun());
        }

        private IEnumerator Stun()
        {
            horizontalVelocity = 0;
            isStunned = true;
            yield return new WaitForSeconds(stunTime);
            isStunned = false;
        }
        
        private void Death()
        {
            if (health <= 0)
            {
                horizontalVelocity = 0;
                isDying = true;
                StartCoroutine(Kill());
            }
        }

        private void Attack()
        {
            anim.SetTrigger("Attack");
            attackCooldownCounter = attackCooldown;
        }

        private void AttackRoot()
        {
            Vector2 origin;
            if(direction == 1) origin = new Vector2(horizontalAttackpointPosition + transform.position.x, transform.position.y);
            else origin = new Vector2(transform.position.x - horizontalAttackpointPosition, transform.position.y);
            Collider2D col = Physics2D.OverlapCircle(origin, attackRadius, rootLayer);
            if (!col) return;
            col.GetComponent<RootManager>().TakeDamage(damage);
        }
        
        private IEnumerator Kill()
        {
            yield return new WaitForSeconds(destroyTime);
            Destroy(gameObject);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, stoppingDistance);
            
            Gizmos.color = Color.red;
            Vector2 origin;
            if(direction == 1) origin = new Vector2(horizontalAttackpointPosition + transform.position.x, transform.position.y);
            else origin = new Vector2(transform.position.x - horizontalAttackpointPosition, transform.position.y);
            Gizmos.DrawWireSphere(origin, attackRadius);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Projectile"))
            {
                Projectile proj = col.GetComponent<Projectile>();
                TakeDamage(proj.damage, new Vector2(proj.knockbackDirection.x / 2, proj.knockbackDirection.y / 1.25f));
                Destroy(col.gameObject);
            }
        }
    }
}
