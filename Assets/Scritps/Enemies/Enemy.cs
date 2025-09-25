using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Small,
        Giant
    }

    [Header("GENERIC")]
    [SerializeField] private EnemyType type;
    [Header("STATS")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int maxHealth;
    protected float ogSpeed;
    protected float dashForce = 3f;
    protected float normalSpeed = 1f;
    protected float dashSpeed = 3f;
    [SerializeField] protected float attackDamage = .5f;
    [Header("COMPONENTS")]
    protected Animator animator;
    protected SpriteRenderer sr;
    protected Rigidbody2D rb;
    [Header("FEEDBACK")]
    [SerializeField] protected float flashTime;
    protected bool isFlashing = true;
    [SerializeField] protected float flashElapsedTime, slowElapsedTime;
    [Header("AI")]
    protected Vector2 movePoint;
    private NavMeshAgent navAgent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float detectionRadius = 2.5f;
    [SerializeField] private float attackRadius = 1f;
    private Vector2 randomPoint;
    private Vector2 pointToMove;
    [Header("COMBAT")]
    [SerializeField] private Transform hitPos;
    [SerializeField] private float damageRadius = .8f;
    [SerializeField] private float attackCooldown = 1.5f;
    private float nextAttackTime;
    private bool isDamaging, alreadyDamaged;

    private enum EnemyState
    {
        Idle,
        Following,
        Attacking
    }

    private EnemyState currentState = EnemyState.Idle;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        GameManager.ChangeGamePhase += SetNewMovePoint; 
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;

        ogSpeed = navAgent.speed;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        movePoint = RoomController.main.finalRoomPos;

        StartCoroutine(SetRandomOffset());
    }

    private void Update()
    {
        if (GameManager.currentGamePhase == GamePhase.Exploration)
        {
            switch (currentState)
            {
                case EnemyState.Idle:
                    movePoint = pointToMove;
                    navAgent.speed = .7f;
                    Move();

                    if (Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Player")))
                    {
                        currentState = EnemyState.Following;
                    }

                    break;

                case EnemyState.Following:
                    movePoint = playerTransform.position;
                    navAgent.speed = 1f;
                    Move();

                    if (type == EnemyType.Small) return;

                    if (Physics2D.OverlapCircle(transform.position, attackRadius, LayerMask.GetMask("Player"))){
                        if (Time.time >= nextAttackTime)
                        {
                            navAgent.enabled = false;
                            StartCoroutine(Attack());
                            nextAttackTime = Time.time + attackCooldown;
                            
                            currentState = EnemyState.Attacking;
                        }
                    }

                    break;

                case EnemyState.Attacking:
                    if (!isDamaging || alreadyDamaged) return;

                    if (Physics2D.OverlapCircle(hitPos.position, damageRadius, LayerMask.GetMask("Player")))
                    {
                        playerTransform.gameObject.GetComponent<PlayerCombat>().TakeDamage(attackDamage);
                        alreadyDamaged = true;
                    }
                    
                    break;
            }
        }
        else
        {
            Move();
        }
    }


    protected virtual void Move()
    {
        navAgent.SetDestination(movePoint);
        animator.SetFloat("Speed", navAgent.velocity.magnitude);

        if (navAgent.velocity.x > 0.1)
        {
            sr.flipX = true;
        }
        else if (navAgent.velocity.x < -0.1)
        {
            sr.flipX = false;
        }
    }

    protected virtual IEnumerator Attack()
    {
        Vector2 dashDirection = (playerTransform.position - transform.position).normalized;
        //hitPos.position = (Vector2)transform.position + dashDirection;
        alreadyDamaged = false;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(.8f);

        rb.velocity = dashDirection * dashForce;
        isDamaging = true;
        AudioManager.main.PlayerSpatialSound(AudioManager.main.giantRatAttack, gameObject);

        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
        isDamaging = false;
        navAgent.enabled = true;
        currentState = EnemyState.Idle;
    }

    private IEnumerator SetRandomOffset()
    {
        randomPoint = Random.insideUnitCircle * 1.5f;
        pointToMove = (Vector2)transform.position + randomPoint;     

        yield return new WaitForSeconds(5f);

        StartCoroutine(SetRandomOffset());
    }

    private void SetNewMovePoint()
    {
        if (GameManager.currentGamePhase == GamePhase.Defense)
        {
            movePoint = RoomController.main.finalRoomPos;
        }
    }

    #region COMBAT

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            StartCoroutine(collision.gameObject.GetComponent<PlayerMovement>().Knockback(direction, 3.5f, .2f));
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            StartCoroutine(collision.gameObject.GetComponent<PlayerMovement>().Knockback(direction, 3.5f, .2f));
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(.5f);
        }
    }


    public void TakeDamage(int damage)
    {
        //StartCoroutine(DamageFlash());
        sr.DOColor(Color.red, .1f).SetLoops(2, LoopType.Yoyo);
        AudioManager.main.PlayerSpatialSound(AudioManager.main.fleshImpact, gameObject, .5f);

        currentHealth -= damage;
        
        if (currentHealth <= 0)
        { 
            Die();
        }
    }

    protected void Die()
    {
        FinalScreen.killedEnemies++;
        animator.SetTrigger("Die");
        sr.sortingLayerName = "Background";
        sr.sortingOrder = 1;
        Destroy(GetComponent<Collider2D>());
        Destroy(rb);
        Destroy(navAgent);
        Destroy(this);
    }

    #endregion

    #region EFFECTS

    public IEnumerator SlowEffect()
    {
        slowElapsedTime = 0;

        while (slowElapsedTime < 2)
        {
            slowElapsedTime += Time.deltaTime;
            navAgent.speed = ogSpeed / 2;
            yield return null;
        }

        navAgent.speed = ogSpeed;
    }

    #endregion

    #region FEEDBACK

    protected IEnumerator DamageFlash()
    {
        if (!isFlashing)
        {
            flashElapsedTime = 0;
        }

        isFlashing = true;

        while (flashElapsedTime < flashTime)
        {
            flashElapsedTime += Time.deltaTime;
            sr.material.SetFloat("_FlashAmount", Mathf.Lerp(1f, 0f, (flashElapsedTime / flashTime)));
            yield return null; 
        }

        isFlashing = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .3f);
        Gizmos.DrawSphere(transform.position, attackRadius);

        Gizmos.color = new Color(0, 0, 1, .3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);

        Gizmos.color = new Color(0, 1, 1, .3f);
        Gizmos.DrawSphere(hitPos.position, damageRadius);
    }

    #endregion
}
