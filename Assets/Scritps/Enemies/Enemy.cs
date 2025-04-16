using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int currentHealth, maxHealth;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected float flashTime;
    protected bool flashing = true;
    protected float elapsedTime;
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
        StartCoroutine("DamageFlash");
 
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

    protected IEnumerator DamageFlash()
    {
        if (!flashing)
        {
            elapsedTime = 0;
        }
        flashing = true;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;

            sr.material.SetFloat("_FlashAmount", Mathf.Lerp(1f, 0f, (elapsedTime / flashTime)));

            yield return null; 
        }
        flashing = false;
    }
}
