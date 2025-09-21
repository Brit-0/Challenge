using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected float interactDistance;
    [SerializeField] protected bool canInteract;

    protected string tip = "Aperte \"E\" para interagir";

    protected virtual void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, interactDistance, LayerMask.GetMask("Player")))
        {
            if (!canInteract)
            {
                canInteract = true;
                TipsUIManager.current.SetTip(tip);
            }
        }
        else
        {
            if (canInteract)
            {
                canInteract = false;
                TipsUIManager.current.DisableTip();
            }
        }

        if (canInteract && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    protected virtual void Interact() 
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 255, .3f);
        Gizmos.DrawSphere(transform.position, interactDistance);
    }
}
