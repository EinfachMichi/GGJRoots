using System;
using Enemy;
using UnityEngine;

public class Snail : MonoBehaviour
{
    [SerializeField] private GameObject[] snailHouse;

    private EnemyManager manager;
    private Animator anim;
    private int currentHouseIndex;
    private int lastHouseIndex;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        manager = GetComponent<EnemyManager>();
    }

    private void CrackHouse()
    {
        if (manager.health <= 6)
        {
            if (manager.health <= 6 && manager.health > 4)
            {
                currentHouseIndex = 0;
                lastHouseIndex = 0;
            }
            else if (manager.health <= 4 && manager.health > 2 && currentHouseIndex != 1)
            {
                lastHouseIndex = currentHouseIndex;
                currentHouseIndex++;
            }
            else if (manager.health <= 2 && manager.health > 0 && currentHouseIndex != 2)
            {
                lastHouseIndex = currentHouseIndex;
                currentHouseIndex++;
            }
            else if (manager.health <= 0)
            {
                anim.SetTrigger("Death");
                manager.isDying = true;
                snailHouse[lastHouseIndex].SetActive(false);
                Destroy(gameObject, 1f);
                return;
            } 
            snailHouse[lastHouseIndex].SetActive(false);
            snailHouse[currentHouseIndex].SetActive(true);
            lastHouseIndex = currentHouseIndex;
        }
    }
}