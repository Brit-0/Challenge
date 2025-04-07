using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;
using Random = UnityEngine.Random;

public class TowerLogic : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private TowerData towerData;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject buttons;
    [SerializeField] private SpriteRenderer _renderer;

    public bool active;

    [SerializeField] private float detectionRadius;
    [SerializeField] private Collider2D[] enemiesInRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private int towerLvl = 1, maxLvl;
    private int currentHealth;
    private float distance, closestDistance;
    private GameObject closestEnemy;
    


    private void Start()
    {
        currentHealth = towerData.maxHealth;
    }

    private void Update()
    {
        if (active)
        {
            enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

            if (closestEnemy != null)
            {
                closestDistance = Vector2.Distance(transform.position, closestEnemy.transform.position);
            }

            if (enemiesInRange.Length > 0)
            {
                foreach (Collider2D enemy in enemiesInRange)
                {
                    distance = Vector2.Distance(transform.position, enemy.transform.position);

                    if (closestEnemy == null || distance < closestDistance)
                    {
                        closestEnemy = enemy.gameObject;
                    }

                    print(closestEnemy.name);
                }
            }
            else
            {
                closestEnemy = null;
                closestDistance = 0;
            }
        }    

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow.WithAlpha(.1f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }

    IEnumerator Active()
    {
        yield return new WaitForSecondsRealtime(towerData.shootCooldown);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        if (TowerManager.placeMode) return;

        if (TowerManager.selected != this && towerLvl < 4)
        {
            if (TowerManager.selected != null)
            {
                TowerManager.selected.ToggleMenu();
            }

            buttons.SetActive(true);
            TowerManager.selected = this;
        }
        else
        {
            buttons.SetActive(false);
            TowerManager.selected = null;
        }
    }

    public void UpgradeTower()
    {
        towerLvl++;
        animator.SetTrigger("LevelUp");

        if (towerLvl == maxLvl)
        {
            ToggleMenu();
        }
    }

    public void UpdateTexture()
    {
        _renderer.material.SetTexture("_MainTexture", _renderer.sprite.texture);
    }

    public void SetRandomSkin()
    {
        float randomH = Random.Range(1, 361) / 360f;
        float randomS = Random.Range(30, 101) / 100f;
        float randomV = Random.Range(40, 101) / 100f;
        print("H: " + randomH + " S: " + randomS + " V: " + randomV);
        _renderer.material.SetColor("_SkinColor", Color.HSVToRGB(randomH, randomS, randomV));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Tower"))
        {
            TowerManager.isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Tower"))
        {
            TowerManager.isColliding = false;
        }
    }
}
