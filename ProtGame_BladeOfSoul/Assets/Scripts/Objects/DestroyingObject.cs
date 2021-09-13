using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyingObject : MonoBehaviour
{
    private Animator animObject;

    private void Start()
    {
        animObject = GetComponent<Animator>();
    }

    public void DestroyObject()
    {
        animObject.SetBool("isDestroy", true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
