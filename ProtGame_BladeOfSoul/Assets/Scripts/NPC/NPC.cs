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
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private DialogueManager dialogueManager;

    private Transform currentGoal;

    private Rigidbody2D rbNPC;

    private float currentSpeed;
    private float waitTime;

    private bool canMove;
    private bool isSpeaking;

    private void Start()
    {
        canMove = true;
        currentSpeed = speed;
        waitTime = startWaitTime;
        rbNPC = GetComponent<Rigidbody2D>();
        dialogueObject = GameObject.Find("DialogueBox");

        moveSpot.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY));
    }

    private void Update()
    {
        ProtPatrol();
        //Patroling();
    }

    #region Patrol 
    private void ProtPatrol()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        }
        if (!canMove || Input.GetKeyDown(KeyCode.F))
        {
            transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, 0);
            dialogueTrigger.TriggerDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Space))
            dialogueManager.DisplayNextSentence();
            

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

    

    private void ChangeGoal()
    {
        if (currentPoint == path.Length - 1)
        {
            currentPoint = 0;
            currentGoal = path[0];
        }
        else
        {
            currentPoint++;
            currentGoal = path[currentPoint];
        }
    }
    #endregion

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
