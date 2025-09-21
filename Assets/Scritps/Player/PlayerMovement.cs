using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    [SerializeField] private float moveSpeed = 3f;
    public Animator animator;
    public static bool canMove = true, isIdle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (canMove)
        {
            Vector2 movement = Vector2.zero;

            if (Input.GetKey(KeyCode.W))
            {
                movement.y = +1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                movement.x = -1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movement.y = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movement.x = +1f;
            }

            movement = movement.normalized;

            isIdle = movement.x == 0 && movement.y == 0;

            if (!isIdle)
            {
                rb.velocity = movement * moveSpeed;
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);
                animator.SetBool("IsMoving", true);
            }
            else if (isIdle)
            {
                rb.velocity = Vector2.zero;
                animator.SetBool("IsMoving", false);
            }

            if (movement.x > 0)
            {
                sr.flipX = false;
            }
            else if (movement.x < 0)
            {
                sr.flipX = true;
            }
        }
        else
        {
            //rb.velocity = Vector2.zero;
            animator.SetBool("IsMoving", false);
        }
    }

    public IEnumerator Knockback(Vector2 direction, float force, float disableTime = .5f)
    {
        canMove = false;
        rb.velocity = direction * force;

        yield return new WaitForSeconds(disableTime);

        canMove = true;
        rb.velocity = Vector2.zero;
    }

    public void PlayStepSound()
    {
        int randomStep = Random.Range(0, 3);
        AudioManager.main.PlaySound(AudioManager.main.footsteps[randomStep], 0.2f);
    }

}