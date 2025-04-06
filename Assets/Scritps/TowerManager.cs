using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class TowerManager : MonoBehaviour
{
    public static TowerManager current;

    private GameObject preview;
    private TowerData previewData;
    public static bool placeMode;

    public Grid grid;
    private Vector2 mPos;
    public float cellSize;

    [SerializeField] private GameObject player;
    [SerializeField] private Button tOsssosButton, tPedraButton;

    public static bool canPlace = true, isColliding, isOnGrid, hasTower;

    [SerializeField] private List<GameObject> towersPfs;

    void Awake()
    {
        current = this;
        grid = new Grid(13, 9, cellSize, new Vector2(-32, -22));
    }

    void Update()
    {
        if (placeMode)
        {
            mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            preview.transform.position = ValidateWorldGridPosition(mPos);

            isOnGrid = grid.GetValue(preview.transform.position) != -1;
            hasTower = grid.GetValue(preview.transform.position) == 1;

            if (!isOnGrid || isColliding || hasTower) // N�o est� no grid, nem est� colidindo ou j� possui uma torre no local
            {
                canPlace = false;
                preview.GetComponent<SpriteRenderer>().color = Color.red.WithAlpha(0.3f);
            }
            else
            {
                canPlace = true;
                preview.GetComponent<SpriteRenderer>().color = Color.white.WithAlpha(0.3f);
            }
            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceTower(mPos);
            }
            else if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                ExitPlaceMode();
            }
        }
    }

    public void EnterPlaceMode(InventoryTower tower)
    {
        if (placeMode) return;

        preview = Instantiate(tower.data.towerPf);
        previewData = tower.data;

        preview.GetComponent<SpriteRenderer>().color.WithAlpha(0.3f);

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

        PlayerInventory.current.RemoveTower(previewData);

        preview.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        preview.GetComponent<SpriteRenderer>().color = Color.white;
        preview.GetComponent<Collider2D>().isTrigger = false;

        canPlace = false;
        grid.SetValue(mPos, 1);
    }

    private Vector3 ValidateWorldGridPosition(Vector3 position)
    {
        grid.GetXY(position, out int x, out int y);
        return new Vector3(grid.GetWorldPosition(x, y).x + cellSize / 2, grid.GetWorldPosition(x, y).y + cellSize / 2);
    }
}
