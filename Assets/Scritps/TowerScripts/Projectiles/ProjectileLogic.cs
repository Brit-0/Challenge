using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private TowerData towerData;
    private GameObject targetEnemy;
    private ProjetileType type;
    private ProjectileEffect[] effects;

    public enum ProjetileType
    {
        Follow,
        Area
    };

    public enum ProjectileEffect
    {
        Damage,
        Slow
    };

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetData(TowerData towerData, GameObject targetEnemy, ProjetileType type, ProjectileEffect[] effects)
    {
        this.towerData = towerData;
        this.targetEnemy = targetEnemy;
        this.type = type;
        this.effects = effects;
    }

    private void FixedUpdate()
    {
        Act();
    }

    private void Act()
    {
        switch (type) 
        {
            case ProjetileType.Follow:

                if (targetEnemy)
                {
                    rb.MovePosition(rb.position + ((Vector2)targetEnemy.transform.position - rb.position).normalized * towerData.projectileSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    Destroy(gameObject);
                }

                break;

            case ProjetileType.Area:

                break;
        }

    }

    private void DealEffect(Enemy enemy)
    {
        foreach (ProjectileEffect effect in effects)
        {
            switch (effect)
            {
                case ProjectileEffect.Damage:

                    enemy.TakeDamage(towerData.towerDamage);

                    break;

                case ProjectileEffect.Slow:

                    enemy.StartCoroutine("SlowEffect");

                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower") || collision.CompareTag("Projectile")) return;
        
        if (collision.CompareTag("Enemy"))
        {
            DealEffect(collision.GetComponent<Enemy>());
        }

        Destroy(gameObject);
    }
}
