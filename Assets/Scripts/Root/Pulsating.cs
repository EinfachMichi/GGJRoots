using System;
using UnityEngine;
using UnityEngine.UI;

public class Pulsating : MonoBehaviour
{
    [SerializeField] private float pulsatingSpeed;
    [SerializeField] private Image image;

    private float pulsatingValue = 255;
    private int direction = -1;
    
    private void Update()
    {
        pulsatingValue += direction * Time.deltaTime * pulsatingSpeed;
        if (pulsatingValue <= 128)
        {
            direction = 1;
        }
        else if (pulsatingValue >= 254)
        {
            direction = -1;
        }
        image.color = new Color(pulsatingValue / 255, pulsatingValue / 255, pulsatingValue / 255, 1f);
    }
}
