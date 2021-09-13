using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    move,
    attack
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxHealth = 10;
    [SerializeField] private VectorValue startingPosition;

    private PlayerState currentState;
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;
    private bool isAttacking;

    public float currentHealth { get; private set; }

    private Vector3 direction;

    private void Start()
    {
        currentHealth = maxHealth;
        currentState = PlayerState.move;
        transform.position = startingPosition.initialValue;
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCo());
        }

        if (currentState == PlayerState.move)
        {
            CheckMoveInput();
        }

        CheckDamage();
    }

    private void FixedUpdate()
    {
        Movement();       
    }

    private void CheckDamage()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentHealth -= 1;
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator AttackCo()
    {
        animPlayer.SetBool("isAttacking", true);
        isAttacking = true;
        currentState = PlayerState.attack;
        yield return null;
        animPlayer.SetBool("isAttacking", false);
        yield return new WaitForSeconds(.1f);
        
        currentState = PlayerState.move;
    }


    private void CheckMoveInput()
    {
        direction = Vector3.zero;
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        if (direction != Vector3.zero)
        {
            //Movement();
            animPlayer.SetFloat("moveX", direction.x);
            animPlayer.SetFloat("moveY", direction.y);
            animPlayer.SetBool("isMoving", true);
        }
        else
            animPlayer.SetBool("isMoving", false);
    }


    private void Movement()
    {
        rbPlayer.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }

}
