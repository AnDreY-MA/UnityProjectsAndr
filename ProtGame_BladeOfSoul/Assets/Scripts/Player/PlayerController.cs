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
    [SerializeField] private DataPlayer playerData;
    [SerializeField] private float knockForce;
    [SerializeField] private VectorValue startingPosition;
    [SerializeField] private HeartManager heartManager;

    [Header("Frame Stuff")]
    [SerializeField] private Color flashColor;
    [SerializeField] private Color regularColor;
    [SerializeField] private float flashDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private Collider2D triggerCollider;

    public PlayerController player;

    private PlayerState currentState;
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;
    private SpriteRenderer playerSprite;

    public float currentHealth { get; set; }

    private Vector3 direction;

    private void Start()
    {
        currentHealth = playerData.maxHealth;
        currentState = PlayerState.move;
        transform.position = startingPosition.initialValue;
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
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

        if (Input.GetKeyDown(KeyCode.F5))
        {
            SavePlayer();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            LoadPlayer();
        }

        CheckDamage();

    }

    private void FixedUpdate()
    {
        Movement();       
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        SavePlayerData data = SaveSystem.LoadPlayer();

        currentHealth = data.health;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;

    }

    private void CheckDamage()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentHealth -= 1;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentHealth += 1;
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAttack"))
        {
            StartCoroutine(FlashCo());
            currentHealth -= 1;
            Vector2 difference = transform.position - collision.transform.position;
            transform.position = new Vector2(transform.position.x + difference.x * knockForce, transform.position.y + difference.y * knockForce);            
        }

        if (collision.CompareTag("Heart"))
        {
            currentHealth += 1;
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("HeartUp"))
        {
            heartManager.maxHearts += 0.5f;
            Destroy(collision.gameObject);
        }
    }

    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerCollider.enabled = false;
        while (temp < numberOfFlashes)
        {
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerCollider.enabled = true;
    }

    private IEnumerator AttackCo()
    {
        animPlayer.SetBool("isAttacking", true);
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
        rbPlayer.MovePosition(transform.position + direction * playerData.maxSpeed * Time.deltaTime);
    }

}
