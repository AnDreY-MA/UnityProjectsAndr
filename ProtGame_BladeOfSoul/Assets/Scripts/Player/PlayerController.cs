using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    move,
    attack
}

public class PlayerController : MonoBehaviour
{
    [Header("PlayerStats")]
    [SerializeField] private DataPlayer playerData;
    [SerializeField] private float knockForce;
    [SerializeField] private float dashAmount = 50.0f;
    [SerializeField] private HeartManager heartManager;
    [SerializeField] private GameObject cam;
    

    [Header("Shooting")]
    [SerializeField] private GameObject crosshair;
    [SerializeField] private float crosshairDistance = 1.0f;
    [SerializeField] private float arrowSpeed = 1.0f;
    [SerializeField] private float shootingRecoil = 0f;
    [SerializeField] private float shootRecoilTime = 1.0f;
    //[SerializeField] private float aimingMoveSpeed = 0.5f;

    [Header("Prefabs:")]
    [SerializeField] private GameObject arrowPrefab;

    [Header("Frame Stuff")]
    [SerializeField] private Color flashColor;
    [SerializeField] private Color regularColor;
    [SerializeField] private float flashDuration;
    [SerializeField] private int numberOfFlashes;
    [SerializeField] private Collider2D triggerCollider;

    private float movementSpeed;

    public PlayerController player;

    private GameObject[] players;

    private GameObject[] cameras;

    private PlayerState currentState;
    private Rigidbody2D rbPlayer;
    private Animator animPlayer;
    private SpriteRenderer playerSprite;

    private bool endOfAiming;
    private bool isDash;
    //private bool isAiming;

    public float currentHealth { get; set; }

    public bool isDead { get; private set; }

    private Vector3 direction;
    //private Vector2 mousePos;

    //private Camera camera;

    private NPC npc;

    
    private void Start()
    {
        currentHealth = playerData.maxHealth;
        movementSpeed = playerData.maxSpeed;
        currentState = PlayerState.move;
        rbPlayer = GetComponent<Rigidbody2D>();
        animPlayer = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        isDead = false;

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(cam);
    }

    private void Update()
    {
        //mousePos = camera.ScreenToWorldPoint(Input.mousePosition);

        if (currentState == PlayerState.move)
        {
            CheckMoveInput();
        }
        //CheckMouse();
        CheckSaveAndLoad();
        CheckAttack();
        CheckDamage();
        CheckDash();
        Aim();
        CheckShooting();
        Shoot();

        //mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        Movement();
    }

    #region Dialogue
    private bool InDialogue()
    {
        if (npc != null)
            return npc.DialogueActive();
        else
            return false;
    }
  
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "NPC")
        {
            npc = collision.gameObject.GetComponent<NPC>();

            if (Input.GetKey(KeyCode.F))
            {
                npc.GetComponent<NPC>().ActivateDialogue();
                movementSpeed = 0;
            }
            else
                movementSpeed = playerData.maxSpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        npc = null;
    }
    #endregion

    #region Checks
    private void CheckAttack()
    {
        if (Input.GetMouseButtonDown(0) && currentState != PlayerState.attack)
        {
            StartCoroutine(AttackCo());
        }
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
            isDead = true;
        }

        if (isDead)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void CheckSaveAndLoad()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SavePlayer();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            LoadPlayer();
        }
    }

    private void CheckShooting()
    {
        if (Input.GetMouseButtonUp(1))
        {
            endOfAiming = true;
        }
        else
            endOfAiming = false;
        
        if (endOfAiming)
            shootingRecoil = shootRecoilTime;

        if (shootingRecoil > 0.0f)
            shootingRecoil -= Time.deltaTime;

        /*if (Input.GetMouseButton(1))
            isAiming = true;
        else
            isAiming = false;*/
    }

    private void CheckMoveInput()
    {
        if (!InDialogue())
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
    }

    private void CheckDash()
    {
        if (Input.GetKey(KeyCode.Space))
            isDash = true;
        else
            isDash = false;
    }

    private void CheckMouse()
    {
        Vector3 mouseMovement = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        direction += mouseMovement;
        direction.Normalize();

    }

    #endregion

    #region SaveAndLoad

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        SavePlayerData data = SaveSystem.LoadPlayer();

        currentHealth = data.healthPlayer;

        Vector3 position;
        position.x = data.positionPlayer[0];
        position.y = data.positionPlayer[1];
        position.z = data.positionPlayer[2];
        transform.position = position;

    }

    #endregion

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

    #region Fight

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

    private void Aim()
    {
        if (direction != Vector3.zero)
        {
            crosshair.transform.localPosition = direction * crosshairDistance;
        }
    }

    private void Shoot()
    {
        Vector2 shootingDirection = crosshair.transform.localPosition;
        shootingDirection.Normalize();

        if (endOfAiming)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
            arrow.GetComponent<Rigidbody2D>().velocity = shootingDirection * arrowSpeed;
            arrow.transform.Rotate(0, 0, Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg);
            Destroy(arrow, 2.0f);
        }
    }

    #endregion

    private void Movement()
    {
        if (!isDash)
        {
            rbPlayer.MovePosition(transform.position + direction * movementSpeed * Time.deltaTime);
        }
        else
            rbPlayer.MovePosition(transform.position + direction * dashAmount * Time.deltaTime);
    }

    private void OnLevelWasLoaded(int level)
    {
        FindStartPos();

        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 1)
            Destroy(players[1]);

        cameras = GameObject.FindGameObjectsWithTag("Cameras");
        if (cameras.Length > 1)
            Destroy(cameras[1]);
    }

    private void FindStartPos()
    {
        transform.position = GameObject.FindWithTag("StartPos").transform.position;
    }

}
