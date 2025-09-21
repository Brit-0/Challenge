using UnityEngine;

public class Spawner : Interactable
{
    [SerializeField] private bool isOpen = true;
    public bool isActive;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        tip = "Aperte \"E\" para bloquear a saída";
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
