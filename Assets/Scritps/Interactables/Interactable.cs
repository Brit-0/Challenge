using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] protected float interactDistance;
    [SerializeField] protected bool canInteract;

    protected virtual void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, interactDistance).gameObject.CompareTag("Player"))
        {
            if (!canInteract)
            {
                canInteract = true;
                TipsUIManager.current.SetTip("Aperte \"E\" para interagir");
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
