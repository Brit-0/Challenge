using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestScript : MonoBehaviour
{

    [Header("VARIABLES")]
    [SerializeField] private float proximityDetector = 1.3f;
    [SerializeField] private bool canInteract;
    [SerializeField] private GameObject skillCheck;
    private bool isOpened, isChecking;

    private int[] materialsLoot = new int[3];

    [Header("PREFABS")]
    [SerializeField] private GameObject[] materialPrefabs = new GameObject[2];

    [Header("REFERECNCES")]
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
    }

    public void Open()
    {
        isOpened = true;
        SetLoot();
        //animator.SetTrigger("Open");

        for (int materialIndex = 0; materialIndex < 2; materialIndex++)
        {
            for (int i = 0; i < materialsLoot[materialIndex]; i++)
            {
                Vector2 randomPoint = Random.insideUnitCircle;
                Vector2 randomPos = transform.position + new Vector3(randomPoint.x, randomPoint.y , 0) * 1.3f;
                GameObject newMaterial = Instantiate(materialPrefabs[materialIndex], randomPos, Quaternion.identity);
            }
        }
    }

    private void SetLoot()
    {
        materialsLoot[0] = Random.Range(0, 3);

        if (materialsLoot[0] == 0)
        {
            materialsLoot[1] = Random.Range(1, 3);
        }
        else
        {
            materialsLoot[1] = Random.Range(0, 3);
        }
        
    }

    /*private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 0, 255, .3f);
        Gizmos.DrawSphere(transform.position, proximityDetector);
    }*/
}
