using UnityEngine;

public class RootManager : MonoBehaviour
{
    [SerializeField] private int health;

    public void TakeDamage(int damage)
    {
        health -= damage;
        print("Health left: " + health);
        if (health <= 0)
        {
            print("Game over");
        }
    }
}
