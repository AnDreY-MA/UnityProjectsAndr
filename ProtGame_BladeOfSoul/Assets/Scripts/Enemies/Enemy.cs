using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    public EntityData entityData;

    [SerializeField] private float knockForce;

    private float currentHealth;

    protected Rigidbody2D enemyRb;

    public virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        currentHealth = entityData.maxHealth;
    }

    public virtual void Update()
    {
        CheckSaveLoad();
    }

    private void CheckSaveLoad()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SaveEnemy();
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            LoadEnemy();
        }
    }

    private void SaveEnemy()
    {
        SaveSystem.SaveEnemy(this);
    }

    private void LoadEnemy()
    {
        SaveEnemyData data = SaveSystem.LoadEnemy();

        Vector3 position;
        position.x = data.positionEnemy[0];
        position.y = data.positionEnemy[1];
        position.z = data.positionEnemy[2];
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            Damage(collision);           
        }
    }

    private void Damage(Collider2D collision)
    {
        Vector2 difference = transform.position - collision.transform.position;
        transform.position = new Vector2(transform.position.x + difference.x * knockForce, transform.position.y + difference.y * knockForce);

        currentHealth -= entityData.damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }


}
