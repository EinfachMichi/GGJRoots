using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnLadyBugs : MonoBehaviour
{
    [SerializeField] private GameObject ladybug;
    [SerializeField] private float spawnCycle;
    [SerializeField] private Transform leftPos, rightPos;

    private void OnEnable()
    {
        StartCoroutine(SpawnLadyBug());
    }

    private IEnumerator SpawnLadyBug()
    {
        yield return new WaitForSeconds(0.2f);
        Vector2 pos = leftPos.position;
        float rand = Random.Range(0f, 1f);
        if (rand >= 0.5f)
        {
            pos = leftPos.position;
        }
        else if (rand <= 0.5f)
        {
            pos = rightPos.position;
        }
        Instantiate(ladybug, pos, Quaternion.identity);
        yield return new WaitForSeconds(spawnCycle);
        StartCoroutine(SpawnLadyBug());
    }
}
