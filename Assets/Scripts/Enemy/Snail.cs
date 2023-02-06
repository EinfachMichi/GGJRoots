using Enemy;
using UnityEngine;

public class Snail : MonoBehaviour
{
    [SerializeField] private GameObject[] snailHouse;

    private EnemyManager manager;
    private Animator anim;
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        manager = GetComponent<EnemyManager>();
    }

    public void CrackHouse()
    {
        switch (manager.health)
        {
            case 8:
                snailHouse[0].SetActive(false);
                snailHouse[1].SetActive(false);
                snailHouse[2].SetActive(false);
                break;
            case 7:
                snailHouse[0].SetActive(false);
                snailHouse[1].SetActive(false);
                snailHouse[2].SetActive(false);
                break;
            case 6:
                snailHouse[0].SetActive(true);
                snailHouse[1].SetActive(false);
                snailHouse[2].SetActive(false);
                break;
            case 5:
                snailHouse[0].SetActive(true);
                snailHouse[1].SetActive(false);
                snailHouse[2].SetActive(false);
                break;
            case 4:
                snailHouse[0].SetActive(false);
                snailHouse[1].SetActive(true);
                snailHouse[2].SetActive(false);
                break;
            case 3:
                snailHouse[0].SetActive(false);
                snailHouse[1].SetActive(true);
                snailHouse[2].SetActive(false);
                break;
            case 2:
                snailHouse[0].SetActive(false);
                snailHouse[1].SetActive(false);
                snailHouse[2].SetActive(true);
                break;
            case 1:
                snailHouse[0].SetActive(false);
                snailHouse[1].SetActive(false);
                snailHouse[2].SetActive(true);
                break;
        }

        if (manager.health <= 0)
        {
            anim.SetTrigger("Death");
            manager.isDying = true;
            snailHouse[0].SetActive(false);
            snailHouse[1].SetActive(false);
            snailHouse[2].SetActive(false);
            Destroy(gameObject, 1f);
        }
    }
}