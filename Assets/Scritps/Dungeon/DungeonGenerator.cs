using System.Collections.Generic;
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

    private GameObject currentRoom, newRoom;
    private Room currentRoomData, newRoomData;

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

    [ContextMenu("Define Rooms")]
    private void DefineRooms()
    {
        if (roomIndex == maxRooms) return;

        if (roomIndex == 0) //START ROOM
        {
            rooms.Add(roomPool[Random.Range(0, roomPool.Count)]);
            currentRoom = rooms[0];
        }
        else
        {
            currentRoom = newRoom;
        }

        rooms.Add(roomPool[Random.Range(0, roomPool.Count)]);

        newRoom = rooms[roomIndex + 1];

        currentRoomData = currentRoom.GetComponent<Room>();
        newRoomData = newRoom.GetComponent<Room>();

        print(currentRoom);
        print(newRoom);

        SpawnRoom();
    }

    private void SpawnRoom()
    {
        if (roomIndex == 0) //START ROOM
        {
            currentRoom = Instantiate(currentRoom, Vector2.zero, Quaternion.identity, roomsGrid);
        }

        newRoom = Instantiate(newRoom, currentRoom.transform.position, Quaternion.identity, roomsGrid);

        print(currentRoom);
        print(newRoom);

        Snap();
    }
        
    private void Snap()
    {
        List<string> availableDirections = currentRoomData.GetDirections();
        print(availableDirections.Count);

        string direction = availableDirections[Random.Range(0, availableDirections.Count)];

        currentRoomData.RemoveDirection(direction);
        newRoomData.RemoveDirection(InvertDirection(direction));

        print(direction);

        Transform snapPointTo, snapPointFrom;

        snapPointTo = FindChildAll(currentRoom, "SnapPoint" + direction);
        snapPointFrom = FindChildAll(newRoom, "SnapPoint" + InvertDirection(direction));

        //REARRANJAR HIERARQUIA DA SALA NOVA

        snapPointFrom.parent = newRoom.transform;

        Transform newTM = newRoom.transform.Find("Tilemap");
        newTM.parent = snapPointFrom;

        Transform snapPoints = newRoom.transform.Find("SnapPoints");
        snapPoints.parent = snapPointFrom;

        //SNAP PARA PONTO CORRETO

        snapPointFrom.Translate(snapPointTo.position - snapPointFrom.position);

        OpenDoors(direction, newTM);
    }

    private void OpenDoors(string direction,Transform tm)
    {
        Tilemap currentTileMap = FindChildAll(currentRoom, "Tilemap").GetComponent<Tilemap>();
        Tilemap newTileMap = tm.GetComponent<Tilemap>();

        Room currentRoomData = currentRoom.GetComponent<Room>();
        Room newRoomData = newRoom.GetComponent<Room>();

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
                break;
            case "Right":
                currentTileMap.SetTilesBlock(currentRoomData.boundsRight, new TileBase[2]);
                newTileMap.SetTilesBlock(newRoomData.boundsLeft, new TileBase[2]);
                break;
        }

        roomIndex++;

        //DefineRooms();
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


    /*private void ResetDoors()
    {
        if (tm.ContainsTile(doorUpLeft) ||
            tm.ContainsTile(doorUpRight) ||
            tm.ContainsTile(doorDownLeft) ||
            tm.ContainsTile(doorDownRight))
        {
            tm.SwapTile(doorUpLeft, wallUp);
            tm.SwapTile(doorUpRight, wallUp);
            tm.SwapTile(doorDownLeft, wallDown);
            tm.SwapTile(doorDownRight, wallDown);
        }
    }*/
}
