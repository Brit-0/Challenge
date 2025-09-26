using UnityEngine;

public class Lever : Interactable
{
    private enum LeverType
    {
        Door,
        Trap
    }

    [SerializeField] private GameObject linkedObject;
    [SerializeField] private LeverType type;
    private Animator animator;

    private bool hasBeenPushed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (!hasBeenPushed)
        {
            base.Update();
        }
    }

    protected override void Interact()
    {
        if (type == LeverType.Door)
        {
            linkedObject.GetComponent<Animator>().SetBool("isOpen", true);

            AudioManager.main.PlaySpatialSound(AudioManager.main.gate, linkedObject, 1f);
        }
        else if (type == LeverType.Trap)
        {
            linkedObject.GetComponent<Collider2D>().enabled = false;
            linkedObject.GetComponent<Animator>().SetBool("isOn", false);
        }

        AudioManager.main.PlaySound(AudioManager.main.lever);
        TipsUIManager.current.DisableTip();
        animator.SetBool("isDown", true);

        hasBeenPushed = true;

    }
}
