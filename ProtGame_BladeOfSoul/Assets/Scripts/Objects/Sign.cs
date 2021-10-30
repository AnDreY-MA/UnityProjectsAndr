using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : MonoBehaviour
{
    [SerializeField] private GameObject dialogBox;
    [SerializeField] private Text dialogText;
    [SerializeField] private string dialog;

    private bool dialogActive;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && dialogActive)
        {
            if (dialogBox.activeInHierarchy)
                dialogBox.SetActive(false);
            else
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
                
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogActive = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogActive = false;
            dialogBox.SetActive(false);
        }

    }

}
