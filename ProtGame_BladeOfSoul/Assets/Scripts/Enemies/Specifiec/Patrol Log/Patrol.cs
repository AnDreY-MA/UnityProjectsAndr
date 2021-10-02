using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : Enemy
{
    [SerializeField] private float speed = 5;
    [SerializeField] private float startWaitTime;

    [Header("ProtPatrol")]
    [SerializeField] Transform moveSpot;
    [SerializeField] private float minX;
    [SerializeField] private float minY;
    [SerializeField] private float maxX;
    [SerializeField] private float maxy;

    [Header("Patrolling")]
    [SerializeField] Transform[] path;
    [SerializeField] private int currentPoint;
    [SerializeField] private float roundingDistance;

    private Transform currentGoal;

    private float waitTime;

    public override void Start()
    {
        waitTime = startWaitTime;

        moveSpot.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxy));
    }

    private void Update()
    {
        ProtPatrol();
        //Patroling();
    }

    private void ProtPatrol()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpot.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, moveSpot.position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                moveSpot.position = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxy));
                waitTime = startWaitTime;
            }
            else
                waitTime -= Time.deltaTime;
        }
    }

    private void Patroling()
    {
        if (Vector3.Distance(transform.position, path[currentPoint].position) > roundingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[currentPoint].position, entityData.moveSpeed * Time.deltaTime);

        }
        else
            ChangeGoal();
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
