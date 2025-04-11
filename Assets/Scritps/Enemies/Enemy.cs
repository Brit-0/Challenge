using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int currentHealth, maxHealth;
    protected float speed;

    [SerializeField] Rigidbody2D rb;

    protected virtual void Move()
    {

    }
}
