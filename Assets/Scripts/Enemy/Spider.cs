using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Spider : MonoBehaviour
{
    public int damage;
    public int maxHealth;
    public float attackCooldown;
    public float bulletSpeed;
    public GameObject spiderWeb;
    public Transform attackPoint;
    [HideInInspector] public bool left;
    [HideInInspector] public bool right;
    
    private BoxCollider2D collider;
    private Animator anim;
    private bool isDead;
    private int direction;
    private int health;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        health = maxHealth;
        if (transform.position.x < 0) direction = -1;
        else if (transform.position.x > 0) direction = 1;
        transform.localScale = new Vector3(direction, 1, 1);
        StartCoroutine(Attack());
    }

    private void TakeDamage(int damage)
    {
        health -= damage;
        AudioManager.instance.Play("Hit", AudioManager.instance.effectSounds);
        if (health <= 0)
        {
            Death();
        }
    }

    public void Heal(int heal)
    {
        health += heal;
        if (health >= maxHealth) health = maxHealth;
    }

    private void Death()
    {
        isDead = true;
        collider.enabled = false;
        anim.SetTrigger("Death");
        if (right) EnemySpawnerManager.instance.rightFree = true;
        else if (left) EnemySpawnerManager.instance.leftFree = true;
    }

    private void Kill()
    {
        Destroy(gameObject);
    }
    
    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackCooldown);
        if(isDead) yield break;

        GameObject obj = Instantiate(spiderWeb, attackPoint.position, Quaternion.identity);
        Vector2 dir = ((new Vector3(0, 0) - transform.position)).normalized;
        obj.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed, ForceMode2D.Impulse);
        obj.GetComponent<SpiderWeb>().damage = damage;
        StartCoroutine(Attack());
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Projectile"))
        {
            Projectile proj = col.GetComponent<Projectile>();
            TakeDamage(proj.damage);
            Destroy(col.gameObject);
        }
    }
}
