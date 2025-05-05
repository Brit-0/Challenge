using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    public Animator animator;
    public static bool canMove = true;
    [SerializeField] private Vector2 movement;

    void Update()
    {
        if (canMove)
        {
            movement.x = Input.GetAxisRaw("Horizontal"); //A & D
            movement.y = Input.GetAxisRaw("Vertical"); //W & S

            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
        }
    }
}
