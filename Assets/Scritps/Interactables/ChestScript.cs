using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class ChestScript : Interactable
{

    [Header("VARIABLES")]
    [SerializeField] private GameObject skillCheck;
    private bool isOpened;

    private int[] materialsLoot = new int[3];

    [Header("PREFABS")]
    [SerializeField] private GameObject[] materialPrefabs = new GameObject[2];

    [Header("REFERECNCES")]
    private Animator animator;
    public static ChestScript currentChest;
    private Tilemap floorTilemap;
    public int failuresCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        floorTilemap = GameObject.Find("Walkable").GetComponent<Tilemap>();
    }

    protected override void Interact()
    {
        StartCheck();
    }

    protected override void Update()
    {
        if (!isOpened)
        {
            base.Update();
        }
    }

    void StartCheck()
    {
        //isChecking = true;
        skillCheck.SetActive(true);
        TipsUIManager.current.SetTip("Aperte \"E\" quando o indicador estiver na seção verde");
        PlayerMovement.canMove = false;
        currentChest = this;
    }

    public void Open()
    {
        isOpened = true;
        SetLoot();
        animator.SetTrigger("Open");
    }

    public void SpawnMaterials()
    {
        for (int materialIndex = 0; materialIndex < 3; materialIndex++)
        {
            for (int i = 0; i < materialsLoot[materialIndex]; i++)
            {
                GameObject newMaterial = Instantiate(materialPrefabs[materialIndex], transform.position, Quaternion.identity);
                Vector2 pos = GetRandomSpawn();
                newMaterial.transform.DOMove(pos, .5f).SetEase(Ease.OutSine);
            }
        }
    }

    private Vector2 GetRandomSpawn()
    {
        Vector2 randomPos = (Vector2)transform.position + Random.insideUnitCircle * 0.8f;

        while (!floorTilemap.HasTile(floorTilemap.WorldToCell(randomPos)))
        {
            randomPos = (Vector2)transform.position + Random.insideUnitCircle * 0.8f;
        }

        return randomPos;
    }

    private void SetLoot()
    {
        int minLoot = 3;

        int maxLoot = 6;
        maxLoot -= Mathf.RoundToInt(failuresCounter / 1.5f);
        Mathf.Clamp(maxLoot, 3, 6);

        if (maxLoot < 6)
        {
            if (maxLoot == 3)
            {
                minLoot = 1;
            }
            else
            {
                minLoot = 2;
            }
        }

        print("MinLoot: " + minLoot + " / MaxLoot: " + maxLoot);

        materialsLoot[0] = Random.Range(0, maxLoot + 1);
        materialsLoot[1] = Random.Range(0, maxLoot + 1);

        if (materialsLoot[0] == 0 || materialsLoot[1] == 0)
        {
            materialsLoot[2] = Random.Range(minLoot, maxLoot + 1);
        }
        else
        {
            materialsLoot[2] = Random.Range(0, maxLoot + 1);
        }

        print("Pedras: " + materialsLoot[0]);
        print("Madeiras: " + materialsLoot[1]);
        print("Ossos: " + materialsLoot[2]);
    }
}
