using System;
using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI statText;
    [SerializeField] private float lifeTime;
    [SerializeField] private float speed;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Translate(speed * Time.deltaTime * Vector3.up);
    }
}
