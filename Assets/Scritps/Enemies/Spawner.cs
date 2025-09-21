using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Interactable
{
    private bool isOpen = true, isActive = false;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (isActive && isOpen)
        {
            base.Update();
        }
    }

    protected override void Interact()
    {
        animator.SetBool("isOpen", false);
        AudioManager.main.PlaySound(AudioManager.main.barricade);
        isOpen = false;
    }
}
