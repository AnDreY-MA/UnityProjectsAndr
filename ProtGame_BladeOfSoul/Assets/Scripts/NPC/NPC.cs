using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float startWaitTime;

    [Header("ProtPatrol")]
    [SerializeField] Transform moveSpot;
    [SerializeField] private float minX;
    [SerializeField] private float minY;
    [SerializeField] private float maxX;
    [SerializeField] private float maxY;

    [Header("Patrolling")]
    [SerializeField] Transform[] path;
    [SerializeField] private int currentPoint;
    [SerializeField] private float roundingDistance;

    [Header("DialogueSystem")]
    [SerializeField] private GameObject dialogueObject;

    private float waitTime;

    private bool canMove;

    private void Start()
    {
        canMove = true;
        waitTime = startWaitTime;

        moveSpot.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    private void Update()
    {
        ProtPatrol();
    }

    public void ActivateDialogue() { dialogueObject.SetActive(true); }

    public bool DialogueActive() { return dialogueObject.activeInHierarchy; }     

    private void ProtPatrol()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        }
        if (!canMove || Input.GetKeyDown(KeyCode.F))
        {
            transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, 0);
        }            

        if (Vector3.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                moveSpot.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
                waitTime = startWaitTime;
            }
            else
                waitTime -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            canMove = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            canMove = true;
    }
}
