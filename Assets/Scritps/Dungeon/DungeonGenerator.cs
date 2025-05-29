using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;


public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator current;

    [SerializeField] private int roomIndex;

    [SerializeField] private int roomCount, maxRooms;

    [Header("ROOM GRID")]
    [SerializeField] private Transform roomsGrid;

    [Header("DOOR TILES")]
    [SerializeField] private TileBase[] doorTiles = new TileBase[4];

    [Header("WALL TILES")]
    [SerializeField] private Tile wallUp;
    [SerializeField] private Tile wallDown;

    [Header("ROOM POOL")]
    [SerializeField] private List<GameObject> roomPool = new List<GameObject>();

    [Header("CURRENT DUNGEON ROOMS")]
    [SerializeField] List<GameObject> rooms = new List<GameObject>();

    private GameObject currentRoomRef, newRoomRef, currentRoomInst, newRoomInst;
    private Room currentRoomData, newRoomData;
    private List<string> availableDirections;

    [SerializeField] bool nextRoom;

    private void Awake()
    {
        current = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DefineRooms();
        }
    }

    [ContextMenu("Generate")]
    private void DefineRooms()
    {
        if (roomIndex == maxRooms - 1) return;

        if (roomIndex == 0) //START ROOM
        {
            rooms.Add(roomPool[Random.Range(0, roomPool.Count)]);
            currentRoomRef = rooms[0];
        }

        if (nextRoom)
        {
            currentRoomInst = newRoomInst;
            nextRoom = false;
        }
   
        rooms.Add(roomPool[Random.Range(0, roomPool.Count)]);

        newRoomRef = rooms[roomIndex + 1];

        SpawnRoom();
    }

    private void SpawnRoom()
    {
        if (roomIndex == 0) //START ROOM
        {
            currentRoomInst = Instantiate(currentRoomRef, Vector2.zero, Quaternion.identity, roomsGrid);
        }

        newRoomInst = Instantiate(newRoomRef, currentRoomInst.transform.position, Quaternion.identity, roomsGrid);

        currentRoomData = currentRoomInst.GetComponent<Room>();
        newRoomData = newRoomInst.GetComponent<Room>();

        Snap();
    }

    [ContextMenu("Snap")]
    private void Snap()
    {
        availableDirections = currentRoomData.GetDirections();

        string direction = availableDirections[Random.Range(0, availableDirections.Count)];

        currentRoomData.RemoveDirection(direction);
        newRoomData.RemoveDirection(InvertDirection(direction));

        print(direction);


        Transform snapPointTo, snapPointFrom;

        snapPointTo = FindChildAll(currentRoomInst, "SnapPoint" + direction);
        snapPointFrom = FindChildAll(newRoomInst, "SnapPoint" + InvertDirection(direction));

        //REARRANJAR HIERARQUIA DA SALA NOVA

        snapPointFrom.parent = newRoomInst.transform;

        Transform newTM = FindChildAll(newRoomInst, "Tilemap");
        newTM.parent = snapPointFrom;

        Transform snapPoints = FindChildAll(newRoomInst, "SnapPoints"); ;
        snapPoints.parent = snapPointFrom;

        //SNAP PARA PONTO CORRETO

        snapPointFrom.Translate(snapPointTo.position - snapPointFrom.position);

        StartCoroutine(CheckOverlap(direction, availableDirections));
    }

 
    private IEnumerator CheckOverlap(string direction, List<string> availableDirections)
    {
        yield return new WaitForFixedUpdate();

        Transform overlapChecker = FindChildAll(newRoomInst, "OverlapChecker");

        if (overlapChecker.GetComponent<Collider2D>().IsTouchingLayers(LayerMask.GetMask(new string[] { "Checker" })))
        {
            if (availableDirections.Count > 0)
            {
                print("Deu overlapping e rodou de novo");
                Snap();
                yield break;
            }
            else
            {
                print("Deu overlapping e não tinha mais direções");
                print("Geração encerrada");
                Destroy(newRoomInst);
                yield break;
            }
        }
        else
        {
            Destroy(overlapChecker.GetComponent<Rigidbody2D>());
        }

        OpenDoors(direction);
    }

    private void OpenDoors(string direction)
    {
        Tilemap currentTileMap = FindChildAll(currentRoomInst, "Tilemap").GetComponent<Tilemap>();
        Tilemap newTileMap = FindChildAll(newRoomInst, "Tilemap").GetComponent<Tilemap>();

        switch (direction)
        {
            case "Down":
                currentTileMap.SetTilesBlock(currentRoomData.boundsDown, doorTiles);
                newTileMap.SetTilesBlock(newRoomData.boundsUp, doorTiles);
                break;
            case "Left":
                currentTileMap.SetTilesBlock(currentRoomData.boundsLeft, new TileBase[2]);
                newTileMap.SetTilesBlock(newRoomData.boundsRight, new TileBase[2]);
                break;
            case "Up":
                currentTileMap.SetTilesBlock(currentRoomData.boundsUp, doorTiles);
                newTileMap.SetTilesBlock(newRoomData.boundsDown, doorTiles);
                newTileMap.GetComponent<TilemapRenderer>().sortingOrder = currentTileMap.GetComponent<TilemapRenderer>().sortingOrder - 1;
                break;
            case "Right":
                currentTileMap.SetTilesBlock(currentRoomData.boundsRight, new TileBase[2]);
                newTileMap.SetTilesBlock(newRoomData.boundsLeft, new TileBase[2]);
                break;
        }

        if (Random.Range(1,3) == 1 || availableDirections.Count == 0)
        {
            nextRoom = true;
        }

        roomIndex++;
        print("Next room: " + nextRoom);

        DefineRooms();
    }

    private string InvertDirection(string dir)
    {
        if (dir == "Right")
        {
            return "Left";
        }
        else if (dir == "Left")
        {
            return "Right";
        }
        else if (dir == "Up")
        {
            return "Down";
        }
        else if (dir == "Down")
        {
            return "Up";
        }
        else
        {
            return "Incorrect direction";
        }
    }

    private Transform FindChildAll(GameObject obj, string name)
    {
        Transform found = default;
        Transform[] children = obj.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.name == name)
            {
                found = child;
            }
        }

        return found;
    }
}
