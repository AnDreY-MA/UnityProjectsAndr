using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEnemy : Enemy
{
    [SerializeField] private Transform target;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;

    private Animator enemyAnim;


    public override void Start()
    {
        base.Start();

        target = GameObject.FindWithTag("Player").transform;
        enemyAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            enemyAnim.SetBool("wakeUp", true);
            enemyAnim.SetBool("isMoving", true);
            ApplyMovement();
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
        transform.position = Vector3.MoveTowards(transform.position, target.position, entityData.moveSpeed * Time.deltaTime);
    }

}
