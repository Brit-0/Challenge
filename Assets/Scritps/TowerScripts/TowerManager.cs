using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using TMPro;
using TreeEditor;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public static TowerManager current;

    public static TowerLogic selected;

    private GameObject preview;
    private TowerData previewData;
    public static bool placeMode;
    private GameObject currentCard;

    [SerializeField] List<GameObject> tabs;

    public Grid grid;
    private Vector2 mPos;
    [SerializeField] private float cellSize;

    public static bool canPlace = true, isColliding, isOnGrid, hasTower, isTabOpened;

    void Awake()
    { 
        current = this;
        grid = new Grid(13, 9, cellSize, new Vector2(-8, -4));
    }

    void Update()
    {
        if (placeMode)
        {
            SetCanPlace(); //Definir se pode colocar

            if (Input.GetMouseButtonDown(0) && canPlace) //Colocar a torre
            {
                PlaceTower(mPos);  
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) //Sair do modo de preview
            {
                ExitPlaceMode();
            }
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && selected != null) //Desselecionar todas as torres
        {
            selected.ToggleMenu();
            selected = null;
        }
    }

    private void SetCanPlace()
    {
        mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        preview.transform.position = ValidateWorldGridPosition(mPos); //Snap para o grid

        isOnGrid = grid.GetValue(preview.transform.position) != -1; //Define se está no grid
        hasTower = grid.GetValue(preview.transform.position) == 1; //Define se tem torre na posição

        if (!isOnGrid || isColliding || hasTower || isTabOpened) // Não está no grid, nem está colidindo ou já possui uma torre no local
        {
            canPlace = false;
            preview.GetComponent<SpriteRenderer>().material.SetColor("_PlaceColor", Color.red);
        }
        else
        {
            canPlace = true;
            preview.GetComponent<SpriteRenderer>().material.SetColor("_PlaceColor", Color.black);
        }

        if (hasTower)
        {
            preview.GetComponent<SpriteRenderer>().material.SetFloat("_PlaceAlpha", 1f);
        }
        else
        {
            preview.GetComponent<SpriteRenderer>().material.SetFloat("_PlaceAlpha", .3f);
        }
    }

    public void EnterPlaceMode(InventoryTower tower, GameObject card)
    {
        if (placeMode) return;

        currentCard = card;
        currentCard.GetComponent<Animator>().SetBool("Selected", true);

        preview = Instantiate(tower.data.towerPf);
        previewData = tower.data;

        placeMode = true;
    }

    public void ExitPlaceMode()
    {
        if (!placeMode) return;

        currentCard.GetComponent<Animator>().SetBool("Selected", false);

        Destroy(preview);

        placeMode = false;
    }

    void PlaceTower(Vector2 constructionPoint)
    {
        placeMode = false;
        canPlace = false;
        PlayerInventory.current.RemoveTower(previewData); //Remover torre do inventário
        preview.GetComponent<TowerLogic>().StartCoroutine("SetActive"); //Inicializar torre

        grid.SetValue(mPos, 1); //Definir no grid que existe torre nesse quadrado
    }


    private Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return new Vector3(grid.GetWorldPosition(x, y).x + cellSize / 2, grid.GetWorldPosition(x, y).y + cellSize / 2);
    }
}
