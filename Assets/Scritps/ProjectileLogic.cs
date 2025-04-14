using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rb;

    private TowerData towerData;
    public GameObject targetEnemy;

    public void SetData(TowerData towerData, GameObject targetEnemy)
    {
        this.towerData = towerData;
        this.targetEnemy = targetEnemy;
    }

    private void FixedUpdate()
    {
        if (targetEnemy)
        {
            rb.MovePosition(rb.position + ((Vector2)targetEnemy.transform.position - rb.position).normalized * towerData.projectileSpeed * Time.fixedDeltaTime);
        }
        else
        {
            Destroy(gameObject);
        }   
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tower") || collision.CompareTag("Projectile")) return;
        
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Enemy>().TakeDamage(towerData.towerDamage);
        }

        Destroy(gameObject);
    }
}
