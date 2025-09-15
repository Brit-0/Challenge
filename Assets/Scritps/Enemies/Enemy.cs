using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;

    [Header("STATS")]
    [SerializeField] protected int currentHealth;
    [SerializeField] protected int maxHealth;
    protected float ogSpeed;
    protected float dashForce = 3f;
    protected float normalSpeed = 1f;
    protected float dashSpeed = 3f;
    [Header("COMPONENTS")]
    protected Animator animator;
    protected SpriteRenderer sr;
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
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;

        ogSpeed = navAgent.speed;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

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
                    print("Idle");
                    Move();

                    if (Physics2D.OverlapCircle(transform.position, detectionRadius, LayerMask.GetMask("Player")))
                    {
                        currentState = EnemyState.Following;
                    }

                    break;

                case EnemyState.Following:
                    movePoint = playerTransform.position;
                    navAgent.speed = 1f;
                    print("Following");
                    Move();

                    if (Physics2D.OverlapCircle(transform.position, attackRadius, LayerMask.GetMask("Player"))){
                        navAgent.enabled = false;
                        StartCoroutine(Attack());
                        currentState = EnemyState.Attacking;
                    }

                    break;

                case EnemyState.Attacking:
                    
                    break;
            }
        }
    }


    protected virtual void Move()
    {
        //rb.MovePosition(Vector2.MoveTowards(rb.position, movePoint, moveSpeed * Time.fixedDeltaTime));
        navAgent.SetDestination(movePoint);
    }

    protected virtual IEnumerator Attack()
    {
        Vector2 dashDirection = (playerTransform.position - transform.position).normalized;

        yield return new WaitForSeconds(1.5f);

        rb.velocity = dashDirection * dashForce;

        yield return new WaitForSeconds(0.5f);

        rb.velocity = Vector2.zero;
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

    public void TakeDamage(int damage)
    {
        StartCoroutine("DamageFlash");
 
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
        }
    }

    protected void Die()
    {
        Destroy(gameObject);
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
        Gizmos.color = Color.red.WithAlpha(.3f);
        Gizmos.DrawSphere(transform.position, attackRadius);

        Gizmos.color = Color.blue.WithAlpha(.3f);
        Gizmos.DrawSphere (transform.position, detectionRadius);
    }

    #endregion
}
