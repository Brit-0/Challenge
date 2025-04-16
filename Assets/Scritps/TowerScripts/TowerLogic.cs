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
    [SerializeField] private GameObject menu, upgradeButton;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Transform[] shootPoints = new Transform[3];
    [SerializeField] private List<Transform> activeShootPoints;
    [SerializeField] private GameObject detectionCircle, circleMask;

    public bool active;

    [SerializeField] private Collider2D[] enemiesInRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int towerLvl = 1, maxLvl;
    private int currentHealth, damage;
    private float distance, closestDistance;

    private GameObject closestEnemy;

    private void Awake()
    {
        damage = towerData.towerDamage;
    }

    private void Start()
    {
        currentHealth = towerData.maxHealth;
        activeShootPoints.Add(shootPoints[0]);
        SetCircleSize();
    }

    private void Update()
    {
        if (active)
        {
            enemiesInRange = Physics2D.OverlapCircleAll(transform.position, towerData.detectionRadius, enemyLayer);

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

                    //print(closestEnemy.name);
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

    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, towerData.detectionRadius);
    }
    */

    private void SetCircleSize()
    {
        detectionCircle.transform.localScale = new Vector3(towerData.detectionRadius, towerData.detectionRadius, towerData.detectionRadius) * 20;
        circleMask.transform.localScale = (new Vector3(towerData.detectionRadius * 20 - .5f, towerData.detectionRadius * 20 - .5f, towerData.detectionRadius * 20 - .5f));
    }

    public IEnumerator Active()
    {
        animator.SetTrigger("Click");
        yield return new WaitForSeconds(.1f);
        active = true;
        //print("Torre ativa!");

        while (active)
        {
            yield return new WaitForSecondsRealtime(towerData.shootCooldown);
            if (closestEnemy)
            {
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        foreach (Transform point in activeShootPoints)
        {
            GameObject projectile = Instantiate(towerData.projectilePf, point.position, Quaternion.identity);
            projectile.GetComponent<ProjectileLogic>().SetData(towerData, closestEnemy);
        }
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
        if (TowerManager.placeMode || !active) return;

        if (TowerManager.selected != this)
        {
            if (TowerManager.selected != null)
            {
                TowerManager.selected.ToggleMenu(); //Fechar menu da outra torre selecionada
            }
            animator.SetTrigger("Click");
            menu.SetActive(true);
            TowerManager.selected = this; //Definir essa como a torre selecionada
        }
        else
        {
            menu.SetActive(false);
            TowerManager.selected = null;
        }
    }

    public void UpgradeTower()
    {
        towerLvl++;
        animator.SetInteger("Level", towerLvl);

        if (towerLvl == maxLvl)
        {
            upgradeButton.SetActive(false);
        }
        else
        {
            activeShootPoints.Add(shootPoints[towerLvl - 1]);
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

        if (!collision.CompareTag("Tower") && !active)
        {
            TowerManager.isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Tower") && !active)
        {
            TowerManager.isColliding = false;
        }
    }
}
