using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 knockbackDirection;
    public int damage;

    private void Start()
    {
        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("SpiderWeb"))
        {
            Destroy(gameObject);
        }
    }
}
