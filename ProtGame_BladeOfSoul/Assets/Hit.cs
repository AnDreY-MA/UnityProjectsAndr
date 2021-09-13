using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    private bool changeColor;
    private void Update()
    {
        if (changeColor)
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(255, 255, 255);
            print("Yes");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            changeColor = true;   
        }
    }
}
