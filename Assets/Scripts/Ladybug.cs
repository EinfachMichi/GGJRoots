using System;
using UnityEngine;

public class Ladybug : MonoBehaviour
{
    [SerializeField] private float speed;
    
    private Vector2 lookDir;
    private float verticalDirection;
    private int vertDir;
    
    private void Start()
    {
        if (transform.position.x < 0)
        {
            vertDir = 1;
            lookDir = Vector2.right;
        }
        else if (transform.position.x > 0)
        {
            vertDir = 1;
            lookDir = Vector2.left;
        }
        
        GetComponent<SpriteRenderer>().flipX = lookDir.x == 1;
    }

    private void Update()
    {
        verticalDirection += vertDir * Time.deltaTime * speed;
        if (verticalDirection >= 1)
        {
            vertDir = -1;
        }
        else if (verticalDirection <= -1)
        {
            vertDir = 1;
        }

        lookDir.y = verticalDirection;
        transform.Translate( speed * Time.deltaTime * lookDir);
    }
}
