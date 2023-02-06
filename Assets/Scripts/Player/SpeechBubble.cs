using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private float lifeTime;
    
    void Start()
    {
        Destroy(gameObject, lifeTime);       
    }
}
