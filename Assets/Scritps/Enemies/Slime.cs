using UnityEngine;


public class Slime : Interactable
{
    private int damageTaken;
    [SerializeField] private SpriteRenderer skelRenderer;
    [SerializeField] private GameObject particle;
    [SerializeField] private Transform particlePoint;

    private float nextInteractTime;
    private float interactCooldown = .8f;

    private int damageNeeded;

    private void Awake()
    {
        tip = "Aperte \"E\" para deixar a slime te morder";
        damageNeeded = Random.Range(5, 8);
    }

    protected override void Interact()
    {
        if (Time.time < nextInteractTime) return;

        nextInteractTime = Time.time + interactCooldown;
        damageTaken++;
        PlayerCombat.main.TakeDamage(1);
        AudioManager.main.PlaySound(AudioManager.main.slimeAttack);

        if (damageTaken == damageNeeded)
        {
            skelRenderer.color = Color.green;
            PlayerInput.shootCooldown = .8f;
            AudioManager.main.PlaySound(AudioManager.main.slimeExplosion);
            Instantiate(particle, particlePoint.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
