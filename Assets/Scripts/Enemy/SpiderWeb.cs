using System;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D col)
    {
        Destroy(gameObject);
    }
}
