using UnityEngine;

public class Lever : Interactable
{
    [SerializeField] private GameObject linkedDoor;

    protected override void Update()
    {
        base.Update();
    }

    protected override void Interact()
    {
        linkedDoor.GetComponent<Animator>().SetTrigger("Open");
    }
}
