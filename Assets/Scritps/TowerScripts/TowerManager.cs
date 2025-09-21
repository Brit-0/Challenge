using NavMeshPlus.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    [SerializeField] private Transform gridStartingTransform;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private NavMeshSurface navMesh;

    public static bool canPlace = true, isColliding, isOnGrid, hasTower;

    void Awake()
    { 
        current = this;
        //grid = new Grid(50, 50, cellSize, gridStartingTransform.position);
    }

    void Update()
    {
        if (placeMode)
        {
            //SetCanPlace(); //DEFINIR SE PODE COLOCAR
            SetTilemapCanPlace();

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
            preview.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, .5f);
        }
        else
        {
            canPlace = true;
            preview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
        }

        if (hasTower)
        {
            preview.GetComponent<SpriteRenderer>().color = Color.red;
            preview.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            preview.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    private void SetTilemapCanPlace()
    {
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        Vector3Int cellPosition = floorTilemap.LocalToCell(hit.point);
        preview.transform.position = floorTilemap.GetCellCenterLocal(cellPosition);*/

        preview.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isColliding || hasTower || UIHandler.isTabOpened) // NÃO ESTÁ NO GRID, NEM ESTÁ COLIDINDO OU JÁ POSSUI UMA TORRE NO LOCAL
        {
            canPlace = false;
            preview.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, .75f);
        }
        else
        {
            canPlace = true;
            preview.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .75f);
        }

        if (hasTower)
        {
            preview.GetComponent<SpriteRenderer>().color = Color.red;
            preview.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        else
        {
            preview.GetComponent<SpriteRenderer>().sortingOrder = 0;
        }
    }

    private bool CheckForPlacementSpace()
    {
        if (Physics2D.OverlapCircle(transform.position, 2f))
        {
            return false;
        }
        else
        {
            return true;
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
        preview.GetComponent<SpriteRenderer>().color = Color.white;
        preview.GetComponent<TowerLogic>().StartCoroutine("SetActive"); //INICIALIZAR TORRE
        Destroy(preview.GetComponent<CapsuleCollider2D>());
        //navMesh.BuildNavMesh();
    }


    private Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return new Vector3(grid.GetWorldPosition(x, y).x + cellSize / 2, grid.GetWorldPosition(x, y).y + cellSize / 2);
    }
}
