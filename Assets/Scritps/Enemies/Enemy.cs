using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;

    [SerializeField] protected int currentHealth, maxHealth;
    [SerializeField] protected float moveSpeed;
    protected float ogSpeed;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected float flashTime;
    protected bool flashing = true, slowed;
    protected float flashElapsedTime;
    protected Vector2 movePoint;
    

    

    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        ogSpeed = moveSpeed;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    public IEnumerator SlowEffect()
    {
        if (slowed)
        {
            moveSpeed = ogSpeed;
            yield break;
        }
        slowed = true;
        moveSpeed /= 2;
        yield return new WaitForSecondsRealtime(2f);
        moveSpeed *= 2;
        slowed = false;
    }

    protected void Die()
    {
        Destroy(gameObject);
    }

    protected IEnumerator DamageFlash()
    {
        if (!flashing)
        {
            flashElapsedTime = 0;
        }

        flashing = true;

        while (flashElapsedTime < flashTime)
        {
            flashElapsedTime += Time.deltaTime;
            sr.material.SetFloat("_FlashAmount", Mathf.Lerp(1f, 0f, (flashElapsedTime / flashTime)));
            yield return null; 
        }

        flashing = false;
    }
}
