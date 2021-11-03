using System.Collections;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float speedEnemy;
    [SerializeField] private float knockForce;

    [SerializeField] private float chaseRadius;
    [SerializeField] private float attackRadius;

    private Rigidbody2D rbEnemy;

    private Animator animEnemy;

    private Transform targetPlayer;

    public virtual void Start()
    {
        rbEnemy = GetComponent<Rigidbody2D>();
        animEnemy = GetComponent<Animator>();
        targetPlayer = FindObjectOfType<PlayerController>().transform;
    }

    private void Update()
    {
        CheckDistance();
        CheckAttack();
    }

    private void CheckDistance()
    {
        if (targetPlayer != null)
        {
            if (Vector2.Distance(targetPlayer.position, transform.position) <= chaseRadius && Vector2.Distance(targetPlayer.position, transform.position) > attackRadius)
            {
                animEnemy.SetBool("isMoving", true);
                Movement();
            }
            else
                animEnemy.SetBool("isMoving", false);
        }
    }

    private void CheckAttack()
    {
        if (Vector3.Distance(targetPlayer.position, transform.position) <= chaseRadius && Vector3.Distance(targetPlayer.position, transform.position) <= attackRadius)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        animEnemy.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        animEnemy.SetBool("isAttack", false);
    }

    private void Movement()
    {
        animEnemy.SetFloat("moveX", targetPlayer.position.x - transform.position.x);
        animEnemy.SetFloat("moveY", targetPlayer.position.y - transform.position.y);
        transform.position = Vector3.MoveTowards(transform.position, targetPlayer.position, speedEnemy * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
            Damage(collision);
    }

    private void Damage(Collider2D collision)
    {
        Vector2 difference = transform.position - collision.transform.position;
        transform.position = new Vector2(transform.position.x + difference.x * knockForce, transform.position.y + difference.y * knockForce);
    }
}
