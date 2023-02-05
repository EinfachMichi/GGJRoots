using System;
using UnityEngine;
using UnityEngine.UI;

public class RootManager : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite[] heartIcons;
    [SerializeField] private Animator playerAnim;
    [SerializeField] private Animator rootAnim;

    private int first = 0, second = 1, third = 2, fourth = 3;
    private bool isDead;

    private void Start()
    {
        rootAnim.SetInteger("Health", health);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        health -= damage;
        if (health <= 0)
        {
            isDead = true;
            playerAnim.SetTrigger("Death");
            FindObjectOfType<PlayerController>().isDead = true;
            FindObjectOfType<PlayerController>().horizontalVelocity = 0;
            playerAnim.SetBool("IsDead", true);
            rootAnim.SetTrigger("Death");
            hearts[first].enabled = false;
            hearts[second].enabled = false;
            hearts[third].enabled = false;
            hearts[fourth].enabled = false;
            return;
        }
        
        rootAnim.SetInteger("Health", health);
        switch (health)
        {
            case 8:
                hearts[first].enabled = true;
                hearts[first].sprite = heartIcons[0];
                hearts[second].enabled = true;
                hearts[second].sprite = heartIcons[0];
                hearts[third].enabled = true;
                hearts[third].sprite = heartIcons[0];
                hearts[fourth].enabled = true;
                hearts[fourth].sprite = heartIcons[0];
                break;
            case 7:
                hearts[first].enabled = true;
                hearts[first].sprite = heartIcons[0];
                hearts[second].enabled = true;
                hearts[second].sprite = heartIcons[0];
                hearts[third].enabled = true;
                hearts[third].sprite = heartIcons[0];
                hearts[fourth].enabled = true;
                hearts[fourth].sprite = heartIcons[1];
                break;
            case 6:
                hearts[first].enabled = true;
                hearts[first].sprite = heartIcons[0];
                hearts[second].enabled = true;
                hearts[second].sprite = heartIcons[0];
                hearts[third].enabled = true;
                hearts[third].sprite = heartIcons[0];
                hearts[fourth].enabled = false;
                break;
            case 5:
                hearts[first].enabled = true;
                hearts[first].sprite = heartIcons[0];
                hearts[second].enabled = true;
                hearts[second].sprite = heartIcons[0];
                hearts[third].enabled = true;
                hearts[third].sprite = heartIcons[1];
                hearts[fourth].enabled = false;
                break;
            case 4:
                hearts[first].enabled = true;
                hearts[first].sprite = heartIcons[0];
                hearts[second].enabled = true;
                hearts[second].sprite = heartIcons[0];
                hearts[third].enabled = false;
                hearts[fourth].enabled = false;
                break;
            case 3:
                hearts[first].enabled = true;
                hearts[first].sprite = heartIcons[0];
                hearts[second].enabled = true;
                hearts[second].sprite = heartIcons[1];
                hearts[third].enabled = false;
                hearts[fourth].enabled = false;
                break;
            case 2:
                hearts[first].enabled = true;
                hearts[first].sprite = heartIcons[0];
                hearts[second].enabled = false;
                hearts[third].enabled = false;
                hearts[fourth].enabled = false;
                break;
            case 1:
                hearts[first].enabled = true;
                hearts[first].sprite = heartIcons[1];
                hearts[second].enabled = false;
                hearts[third].enabled = false;
                hearts[fourth].enabled = false;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("SpiderWeb"))
        {
            TakeDamage(col.GetComponent<SpiderWeb>().damage);
        }
    }
}
