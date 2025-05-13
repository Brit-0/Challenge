using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Rendering;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameObject swordPivot;
    [SerializeField] private PauseMenu pauseMenu;
    private Vector2 mPos, direction;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Attack();
        }
        else if (Input.GetButtonDown("Cancel")) //PAUSE
        {
            pauseMenu.OnClick();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) //SPAWNAR INIMIGOS
        {
            HordeSpawner.current.StartCoroutine("SpawnHorde");
        }
    }

    void Attack()
    {
        /*mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mPos - (Vector2)transform.position).normalized;
        GameObject newAttack = Instantiate(swordPivot, transform.position, Quaternion.identity);
        newAttack.transform.up = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newAttack.transform.rotation = Quaternion.Euler(0, 0, angle);*/
    }
}
