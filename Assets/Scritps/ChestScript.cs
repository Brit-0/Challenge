using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class ChestScript : MonoBehaviour
{

    [SerializeField] private float proximityDetector = 1.3f;
    [SerializeField] private bool canInteract;
    private bool isOpened;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Physics2D.OverlapCircle(transform.position, proximityDetector).gameObject.CompareTag("Player"))
        {
            canInteract = true;
        }
        else
        {
            canInteract = false;
        }

        if (canInteract && !isOpened && Input.GetKeyDown(KeyCode.E))
        {
            Open();
        }
    }

    void Open()
    {
        isOpened = true;
        animator.SetTrigger("isOpened");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 255, .3f);
        Gizmos.DrawSphere(transform.position, proximityDetector);
    }
}
