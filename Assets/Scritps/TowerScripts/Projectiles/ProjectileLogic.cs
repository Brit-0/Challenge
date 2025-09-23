using UnityEngine;

public enum ProjetileType
{
    Follow,
    Area,
    Direct
}

public enum ProjectileEffect
{
    Damage,
    Slow
};

public class ProjectileLogic : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private TowerData towerData;
    private GameObject targetEnemy;
    private Vector2 targetDirection;
    private float directSpeed = 5f;
    private ProjetileType type;
    private ProjectileEffect[] effects;
    private bool isSkelProj;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetData(TowerData towerData, GameObject targetEnemy, ProjetileType type, ProjectileEffect[] effects)
    {
        this.towerData = towerData;
        this.targetEnemy = targetEnemy;
        this.type = type;
        this.effects = effects;
    }

    public void SetDataSkel(Vector2 targetPos)
    {
        this.type = ProjetileType.Direct;
        this.targetDirection = (targetPos - (Vector2)transform.position).normalized;
        this.effects = new ProjectileEffect[1] { ProjectileEffect.Damage };
        isSkelProj = true;
    }

    private void FixedUpdate()
    {
        Act();
    }

    private void Act()
    {
        switch (type) 
        {
            case ProjetileType.Follow:

                if (targetEnemy.GetComponent<Enemy>())
                {
                    rb.MovePosition(rb.position + ((Vector2)targetEnemy.transform.position - rb.position).normalized * towerData.projectileSpeed * Time.fixedDeltaTime);
                }
                else
                {
                    Destroy(gameObject);
                }

                break;

            case ProjetileType.Area:

                break;

            case ProjetileType.Direct:

                rb.MovePosition(rb.position + targetDirection * directSpeed * Time.fixedDeltaTime);

                /*if (Vector2.Distance(targetPos, rb.position) < .05f)
                {
                    Destroy(gameObject);
                }*/

                break;
        }
    }

    private void DealEffect(Enemy enemy)
    {
        foreach (ProjectileEffect effect in effects)
        {
            switch (effect)
            {
                case ProjectileEffect.Damage:

                    if (isSkelProj)
                    {
                        enemy.TakeDamage(10);
                    }
                    else
                    {
                        enemy.TakeDamage(towerData.damage);
                    }

                    break;

                case ProjectileEffect.Slow:

                    enemy.StopCoroutine("SlowEffect");
                    enemy.StartCoroutine("SlowEffect");

                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool isEnemy = false;

        if (!isSkelProj)
        {
            if (collision.CompareTag("Tower") || collision.CompareTag("Projectile")) return;
        }

        if (collision.CompareTag("Enemy"))
        {
            DealEffect(collision.GetComponent<Enemy>());
            isEnemy = true;
        }
        
        if (!collision.CompareTag("Player"))
        {
            if (!isEnemy)
            {
                EffectsManager.main.CreateParticle(Particle.Impact, transform.position);
            }

            if (collision.CompareTag("Torch"))
            {
                collision.gameObject.GetComponent<Animator>().SetBool("isOn", true);
                AudioManager.main.PlayerSpatialSound(AudioManager.main.torchIgnite, collision.gameObject);
            }
            
            Destroy(gameObject);
        }
    }
}
