using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int currentHealth, maxHealth;
    [SerializeField] protected float moveSpeed;
    protected Vector2 movePoint;

    [SerializeField] Rigidbody2D rb;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            movePoint = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private void FixedUpdate()
    {
        Move();
    }

    protected virtual void Move()
    {
        rb.MovePosition(Vector2.MoveTowards(rb.position, movePoint, moveSpeed * Time.fixedDeltaTime));
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }
}
