using UnityEngine;

public class Lever : Interactable
{
    [SerializeField] private GameObject linkedDoor;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Interact()
    {
        linkedDoor.GetComponent<Animator>().SetBool("isOpen", true);
        animator.SetBool("isDown", true);
    }
}
