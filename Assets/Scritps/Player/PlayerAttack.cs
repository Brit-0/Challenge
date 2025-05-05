using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject swordPivot;
    private Vector2 mPos, direction;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Attack();
        }
    }

    void Attack()
    {
        mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mPos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        GameObject newAttack = Instantiate(swordPivot, transform.position, Quaternion.identity);
        newAttack.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
