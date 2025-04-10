using System.Collections;
using System.Collections.Generic;
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
            rb.MovePosition(rb.position + ((Vector2)targetEnemy.transform.position - rb.position).normalized * towerData.projectileSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(rb.position, targetEnemy.transform.position) < .2f)
            {
                Destroy(gameObject);
            }
    }
}
