using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Enemy : MonoBehaviour
{
    public EntityData entityData;

    [SerializeField] private float knockForce;

    private float currentHealth;

    private Rigidbody2D enemyRb;

    public virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        currentHealth = entityData.maxHealth;
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
