using Unity.VisualScripting;
using UnityEngine;

public class FlameTrap : MonoBehaviour
{
    [SerializeField] private float knockbackForce = 1f;
    [SerializeField] private float trapDamage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector2 direction = (collision.transform.position - transform.position).normalized;
            StartCoroutine(collision.gameObject.GetComponent<PlayerMovement>().Knockback(direction, knockbackForce));
            collision.gameObject.GetComponent<PlayerCombat>().TakeDamage(trapDamage);
        }
    }
}
