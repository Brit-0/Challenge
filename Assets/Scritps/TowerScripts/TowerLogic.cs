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
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    [SerializeField] private ParticleSystem ps;

    [SerializeField] private TowerData towerData;
    [SerializeField] private GameObject menu, upgradeButton;
    [SerializeField] private Transform[] shootPoints = new Transform[3];
    [SerializeField] private List<Transform> activeShootPoints;
    [SerializeField] private GameObject detectionCircle, circleMask;

    public bool active;

    [SerializeField] private Collider2D[] enemiesInRange;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int towerLvl = 1, maxLvl;
    private int currentHealth;
    private float distance, closestDistance;

    private GameObject closestEnemy;

    private void Awake()
    {
        //Set references
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        currentHealth = towerData.maxHealth;
        activeShootPoints.Add(shootPoints[0]);
    }

    private void Start()
    {
        sr.material.SetFloat("_PlaceAlpha", .3f);
        SetCircleSize();
    }

    private void Update()
    {
        if (active)
        {
            GetEnemiesInRange();
        }    

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void GetEnemiesInRange()
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
            }
        }
        else
        {
            closestEnemy = null;
            closestDistance = 0;
        }
    }

    public IEnumerator SetActive()
    {
        //Travar posição e ativar colisão
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        GetComponent<Collider2D>().isTrigger = false;

        //Arrumar Shader
        sr.material.SetFloat("_PlaceAlpha", 1);
        sr.material.SetColor("_PlaceColor", Color.black);

        //Animação de Colocar
        animator.SetTrigger("Click");

        yield return new WaitForSeconds(.1f);

        active = true;
        StartCoroutine("ActiveLoop"); //Começar loop ativo da torre
    }

    private IEnumerator ActiveLoop()
    {
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
        ps.Play();
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

    public void SetRandomSkin()
    {
        float randomH = Random.Range(1, 361) / 360f;
        float randomS = Random.Range(30, 101) / 100f;
        float randomV = Random.Range(40, 101) / 100f;
        //print("H: " + randomH + " S: " + randomS + " V: " + randomV);
        sr.material.SetColor("_SkinColor", Color.HSVToRGB(randomH, randomS, randomV));
    }
    private void SetCircleSize()
    {
        detectionCircle.transform.localScale = new Vector3(towerData.detectionRadius, towerData.detectionRadius, towerData.detectionRadius) * 20;
        circleMask.transform.localScale = (new Vector3(towerData.detectionRadius * 20 - .5f, towerData.detectionRadius * 20 - .5f, towerData.detectionRadius * 20 - .5f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Tower") && !active) //Se não for outra torre e não estiver ativo
        {
            TowerManager.isColliding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Tower") && !active) //Se não for outra torre e não estiver ativo
        {
            TowerManager.isColliding = false;
        }
    }
}
