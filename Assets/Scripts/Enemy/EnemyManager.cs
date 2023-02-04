using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] private int health;
        [SerializeField] private float speed;
        [SerializeField] private float stoppingDistance;
        [SerializeField] private float attackCooldown;
        [Space]
        [SerializeField] private float horizontalAttackpointPosition;
        [SerializeField] private float attackRadius;
        [SerializeField] private LayerMask rootLayer;

        private Rigidbody2D rb;
        private Animator anim;
        private int direction;
        private float horizontalVelocity;
        private float attackCooldownCounter;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            if (transform.position.x > 0) direction = -1;
            else if (transform.position.x < 0) direction = 1;

            horizontalVelocity = direction * speed;
        }

        private void FixedUpdate()
        {
            rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
        }

        private void Update()
        {
            if (direction == 1)
            {
                if (transform.position.x >= stoppingDistance)
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

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void Attack()
        {
            anim.SetTrigger("Attack");
            attackCooldownCounter = attackCooldown;
            Vector2 origin;
            if(direction == 1) origin = new Vector2(horizontalAttackpointPosition + transform.position.x, transform.position.y);
            else origin = new Vector2(transform.position.x - horizontalAttackpointPosition, transform.position.y);
            Collider2D col = Physics2D.OverlapCircle(origin, attackRadius, rootLayer);
            if (!col) return;
            col.GetComponent<RootManager>().TakeDamage(damage);
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
    }
}
