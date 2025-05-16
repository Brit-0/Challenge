using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager current;

    public static TowerLogic selected;

    private GameObject preview;
    private TowerData previewData;
    public static bool placeMode;

    public Grid grid;
    private Vector2 mPos;
    [SerializeField] private float cellSize;

    public static bool canPlace = true, isColliding, isOnGrid, hasTower;

    void Awake()
    { 
        current = this;
        grid = new Grid(13, 9, cellSize, new Vector2(-8, -4));
    }

    void Update()
    {
        if (placeMode)
        {
            SetCanPlace(); //DEFINIR SE PODE COLOCAR

            if (Input.GetMouseButtonDown(0) && canPlace) //COLOCAR A TORRE
            {
                PlaceTower(mPos);  
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) //SAIR DO MODO DE PREVIEW
            {
                ExitPlaceMode();
            }
        }

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) && selected != null) //DESSELECIONAR TODAS AS TORRES
        {
            selected.ToggleMenu();
            selected = null;
        }
    }

    private void SetCanPlace()
    {
        mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        preview.transform.position = ValidateWorldGridPosition(mPos); //SNAP PARA O GRID

        isOnGrid = grid.GetValue(preview.transform.position) != -1; //DEFINE SE ESTÁ NO GRID
        hasTower = grid.GetValue(preview.transform.position) == 1; //DEFINE SE TEM TORRE NA POSIÇÃO

        if (!isOnGrid || isColliding || hasTower || UIHandler.isTabOpened) // NÃO ESTÁ NO GRID, NEM ESTÁ COLIDINDO OU JÁ POSSUI UMA TORRE NO LOCAL
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

        preview = Instantiate(tower.data.towerPf);
        previewData = tower.data;

        placeMode = true;
    }

    public void ExitPlaceMode()
    {
        if (!placeMode) return;

        Destroy(preview);

        placeMode = false;
    }

    void PlaceTower(Vector2 constructionPoint)
    {
        placeMode = false;
        canPlace = false;
        PlayerInventory.current.RemoveTower(previewData); //REMOVER TORRE DO INVENTARIO
        preview.GetComponent<TowerLogic>().StartCoroutine("SetActive"); //INICIALIZAR TORRE

        grid.SetValue(mPos, 1); //DEFINIR NO GRID QUE EXISE UMA TORRE NESSE QUADRADO
    }


    private Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return new Vector3(grid.GetWorldPosition(x, y).x + cellSize / 2, grid.GetWorldPosition(x, y).y + cellSize / 2);
    }
}
