using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Patrol : Enemy
{
    [SerializeField] private Transform target;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;

    [Header("Patrol")]
    [SerializeField] private Transform[] path;
    [SerializeField] private int currentPoint;
    [SerializeField] private float roundingDistance;
    [SerializeField] private Transform currentGoal;

    private bool checkPlayer;

    private Animator enemyAnim;


    public override void Start()
    {
        base.Start();

        target = GameObject.FindWithTag("Player").transform;
        enemyAnim = GetComponent<Animator>();
        enemyAnim.SetBool("wakeUp", true);
    }

    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            enemyAnim.SetBool("isMoving", true);
            checkPlayer = true;
            ApplyMovement();
        }
        else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, path[currentPoint].position, entityData.moveSpeed * Time.deltaTime);
                enemyRb.MovePosition(temp);
            }
            else
                ChangeGoal();
        }
        else
        {
            enemyAnim.SetBool("isMoving", false);
        }
    }

    private void ApplyMovement()
    {
        enemyAnim.SetFloat("moveX", target.position.x - transform.position.x);
        enemyAnim.SetFloat("moveY", target.position.y - transform.position.y);
        if (checkPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, entityData.moveSpeed * Time.deltaTime);
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
}
