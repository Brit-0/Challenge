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
        mPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = mPos - (Vector2)transform.position;
        GameObject newAttack = Instantiate(swordPivot, transform.position, Quaternion.LookRotation(direction));
    }
}
