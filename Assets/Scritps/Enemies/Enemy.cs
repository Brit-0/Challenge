using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int currentHealth, maxHealth;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Animator animator;
    protected bool canPlayHitAnim = true;
    protected Vector2 movePoint;

    [SerializeField] Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        if (canPlayHitAnim)
        {
            animator.SetTrigger("Hit");
            canPlayHitAnim = false;
        }

        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    protected void HitAnimDebounce()
    {
        canPlayHitAnim = true;
    }
}
