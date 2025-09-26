using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class TowerLogic : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    protected Animator animator;

    [Header("STATUS")]
    public bool active;

    [Header("GENERIC SETTINGS")]
    [SerializeField] private ParticleSystem ps;
    [SerializeField] protected TowerData towerData;
    [SerializeField] private LayerMask enemyLayer;

    [Header("MENU SETTINGS")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject detectionCircle, circleMask;

    [Header("SHOOT POINTS")]
    [SerializeField] private Transform[] shootPoints = new Transform[3];
    [SerializeField] protected List<Transform> activeShootPoints;

    [Header("REFERENCES")]
    [SerializeField] private SpriteRenderer placementArea;

    private int towerLvl = 1;
    private Collider2D[] enemiesInRange;
    private int currentHealth;
    private float distance, closestDistance;
    protected GameObject closestEnemy;

    private void Awake()
    {
        //SET REFERENCES
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

    #region ACTIVE

    public IEnumerator SetActive()
    {
        //RETIRAR CIRCULO DE AREA
        placementArea.gameObject.SetActive(false);
        GetComponent<Light2D>().enabled = true;

        //TRAVAR POSICÃO E ATIVAR A COLISÃO
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        GetComponent<BoxCollider2D>().enabled = true;

        //ARRUMAR SHADER
        sr.material.SetFloat("_PlaceAlpha", 1);
        sr.material.SetColor("_PlaceColor", Color.black);

        //ANIMAÇÃO DE COLOCAR
        animator.SetTrigger("Click");

        yield return new WaitForSeconds(.1f);

        active = true;
        StartCoroutine("ActiveLoop"); //COMEÇAR LOOP ATIVO DA TORRE
    }

    protected virtual IEnumerator ActiveLoop()
    {
        while (active)
        {
            yield return new WaitForSeconds(towerData.shootCooldown);

            if (closestEnemy)
            {
                Shoot();
            }
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

    protected virtual void Shoot()
    {
        
    }

    #endregion

    #region INTERACTIONS

    public void ToggleMenu()
    {
        if (TowerManager.placeMode || !active) return;

        if (TowerManager.selected != this)
        {
            if (TowerManager.selected != null)
            {
                TowerManager.selected.ToggleMenu(); //FECHAR MENU DA TORRE SELECIONADA
            }
            animator.SetTrigger("Click");
            menu.SetActive(true);
            TowerManager.selected = this; //DEFINIR ESSA COMO A TORRE SELECIONADA
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

        if (towerLvl == towerData.maxLevel)
        {
            upgradeButton.SetActive(false);
        }
        else
        {
            activeShootPoints.Add(shootPoints[towerLvl - 1]);
        }
    }

    #endregion

    #region COSMETICS

    public void SetRandomSkin()
    {
        float randomH = Random.Range(1, 361) / 360f;
        float randomS = Random.Range(30, 101) / 100f;
        float randomV = Random.Range(40, 101) / 100f;
        //print("H: " + randomH + " S: " + randomS + " V: " + randomV);
        sr.material.SetColor("_SkinColor", Color.HSVToRGB(randomH, randomS, randomV));
    }

    #endregion

    #region SETUP

    private void SetCircleSize()
    {
        detectionCircle.transform.localScale = new Vector3(towerData.detectionRadius, towerData.detectionRadius, towerData.detectionRadius) * 20;
        circleMask.transform.localScale = (new Vector3(towerData.detectionRadius * 20 - .5f, towerData.detectionRadius * 20 - .5f, towerData.detectionRadius * 20 - .5f));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !active) //Se não for outra torre e não estiver ativo
        {
            TowerManager.isColliding = true;
            placementArea.color = new Color(1, 0, 0, .4f);

            if (collision.CompareTag("FinalRoom"))
            {
                TipsUIManager.current.SetTip("Construa torres fora da sala da relíquia");
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !active) //Se não for outra torre e não estiver ativo
        {
            TowerManager.isColliding = false;
            placementArea.color = new Color(0, 0, 0, .4f);

            if (collision.CompareTag("FinalRoom"))
            {
                TipsUIManager.current.DisableTip();
            }
        }
    }

    #endregion
}


