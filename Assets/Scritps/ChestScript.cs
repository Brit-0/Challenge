using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ChestScript : MonoBehaviour
{

    [SerializeField] private float proximityDetector = 1.3f;
    [SerializeField] private bool canInteract;
    [SerializeField] private GameObject skillCheck;
    private bool isOpened, isChecking;
    public Animator animator;
    public static ChestScript currentChest;
  
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, proximityDetector).gameObject.CompareTag("Player"))
        {
            canInteract = true;
            if (!isChecking)
            {
                TipsUIManager.current.setTip("Aperte \"E\" para interagir com o baú");
            }
        }
        else
        {
            canInteract = false;
            if (!isChecking)
            {
                TipsUIManager.current.disableTip();
            }
        }

        if (canInteract && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            StartCheck();
        }
    }

    void StartCheck()
    {
        isChecking = true;
        skillCheck.SetActive(true);
        TipsUIManager.current.setTip("Aperte \"E\" quando o indicador estiver na seção verde");
        PlayerMovement.canMove = false;
        currentChest = this;
        //animator.SetTrigger("isOpened");
    }

    public void Open()
    {
        isOpened = true;
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 255, .3f);
        Gizmos.DrawSphere(transform.position, proximityDetector);
    }*/
}
