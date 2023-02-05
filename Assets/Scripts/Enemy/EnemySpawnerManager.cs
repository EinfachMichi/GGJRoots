using System.Collections;
using UnityEngine;

public class EnemySpawnerManager : MonoBehaviour
{
    [SerializeField] private Transform rightPos, leftPos;
    [SerializeField] private Transform spiderRightPos, spiderLeftPos;
    [SerializeField] private GameObject[] enemies;
    
    private int currentWaveIndex;
    
    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        float rand1 = Random.Range(0f, 1f);
        int rand = Random.Range(0, enemies.Length);
        GameObject enemy = Instantiate(enemies[rand]);
        if (rand1 > 0.5f)
        {
            if (enemy.tag == "Spider")
            {
                enemy.transform.position = spiderLeftPos.position;
            }
            else
            {
                enemy.transform.position = leftPos.position;
            }
        }
        else
        {
            if (enemy.tag == "Spider")
            {
                enemy.transform.position = spiderRightPos.position;
            }
            else
            {
                enemy.transform.position = rightPos.position;
            }
        }
        float rand2 = Random.Range(1f, 3f);
        yield return new WaitForSeconds(rand2);
        StartCoroutine(SpawnRoutine());
    }
}
