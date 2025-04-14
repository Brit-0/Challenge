using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int currentHealth, maxHealth;
    protected float speed;

    [SerializeField] Rigidbody2D rb;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    protected virtual void Move()
    {

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
