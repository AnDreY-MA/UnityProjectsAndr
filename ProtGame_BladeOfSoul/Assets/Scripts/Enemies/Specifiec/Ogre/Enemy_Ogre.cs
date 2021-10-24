using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    walking,
    attacking
}

public class Enemy_Ogre : Enemy
{
    [SerializeField] private Transform target;
    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;
    [SerializeField] private float delayBetweenAttack;

    private EnemyState ogreState;

    private Animator enemyAnim;


    public override void Start()
    {
        base.Start();

        target = GameObject.FindWithTag("Player").transform;
        enemyAnim = GetComponent<Animator>();
    }

    public override void Update()
    {
        base.Update();
        CheckDistance();
        CheckAttack();
    }

    private void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            enemyAnim.SetBool("isMoving", true);
            ApplyMovement();
        }
        else
        {
            enemyAnim.SetBool("isMoving", false);
        } 
    }

    private void CheckAttack()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius && Vector3.Distance(target.position, transform.position) <= attackRadius)
        {
            StartCoroutine(AttackCo());
        }
    }

    private IEnumerator AttackCo()
    {
        ogreState = EnemyState.attacking;
        yield return new WaitForSeconds(delayBetweenAttack);
        enemyAnim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        enemyAnim.SetBool("isAttack", false);
    }


    private void ApplyMovement()
    {
        enemyAnim.SetFloat("moveX", target.position.x - transform.position.x);
        enemyAnim.SetFloat("moveY", target.position.y - transform.position.y);
        transform.position = Vector3.MoveTowards(transform.position, target.position, entityData.moveSpeed * Time.deltaTime);
    }

}
