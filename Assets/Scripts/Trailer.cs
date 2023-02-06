using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trailer : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject spawner;
    
    private int cycleIndex;
    
    private void Start()
    {
        StartCoroutine(Cycle());
        spawner.SetActive(false);
        cycleIndex = 0;
    }

    private IEnumerator Cycle()
    {
        if(cycleIndex >= sprites.Length)
        {
            Destroy(gameObject);
            spawner.SetActive(true);
            yield break;
        }
        image.sprite = sprites[cycleIndex];
        cycleIndex++;
        yield return new WaitForSeconds(lifeTime);
        StartCoroutine(Cycle());
    }
}
