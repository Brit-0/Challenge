using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;


public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator current;

    [SerializeField] private int roomCount, maxRooms;

    [Header("DOOR TILES")]
    [SerializeField] private TileBase doorDownLeft;
    [SerializeField] private TileBase doorDownRight;
    [SerializeField] private TileBase doorUpLeft;
    [SerializeField] private TileBase doorUpRight;
    TileBase[] doorTiles;

    [Header("WALL TILES")]
    [SerializeField] private Tile wallUp;
    [SerializeField] private Tile wallDown;

    [Header("ROOM POOL")]
    [SerializeField] private List<RoomData> roomPool = new List<RoomData>();

    [Header("CURRENT DUNGEON ROOMS")]
    [SerializeField] List<GameObject> rooms = new List<GameObject>();

    private string[] directions = new string[4]
    {
        "Right",
        "Left",
        "Up",
        "Down"
    };

    private GameObject currentRoom, otherRoom;

    private void Awake()
    {
        current = this;
        doorTiles = new TileBase[4] { doorDownLeft, doorDownRight, doorUpLeft, doorUpRight };
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
        currentRoom = rooms[0];
        otherRoom = rooms[1];

        Snap();
    }

    private void Snap()
    {
        string direction = directions[Random.Range(0, 4)];
        Transform snapPointFrom = otherRoom.transform.Find("SnapPoints").Find("SnapPoint" + direction);
        Transform snapPointTo = currentRoom.transform.Find("SnapPoints").Find("SnapPoint" + InvertDirection(direction));

        Transform tm = otherRoom.transform.Find("Tilemap");
        tm.parent = snapPointFrom;

        snapPointFrom.Translate(snapPointTo.position - snapPointFrom.position);

        OpenDoors(direction, tm);
    }

    private void OpenDoors(string direction,Transform tm)
    {
        Tilemap currentTileMap = currentRoom.transform.Find("Tilemap").GetComponent<Tilemap>();
        Tilemap otherTileMap = tm.GetComponent<Tilemap>();

        RoomData currentRoomData = currentRoom.GetComponent<Room>().data;
        RoomData otherRoomData = otherRoom.GetComponent<Room>().data;

        if (direction == "Down")
        {
            currentTileMap.SetTilesBlock(currentRoomData.boundsUp, doorTiles);
            otherTileMap.SetTilesBlock(otherRoomData.boundsDown, doorTiles);
        }
        if (direction == "Left")
        {
            currentTileMap.SetTilesBlock(currentRoomData.boundsRight, doorTiles);
            otherTileMap.SetTilesBlock(otherRoomData.boundsLeft, doorTiles);
        }
        if (direction == "Up")
        {
            currentTileMap.SetTilesBlock(currentRoomData.boundsDown, doorTiles);
            otherTileMap.SetTilesBlock(otherRoomData.boundsUp, doorTiles);
        }
        if (direction == "Right")
        {
            currentTileMap.SetTilesBlock(currentRoomData.boundsLeft, doorTiles);
            otherTileMap.SetTilesBlock(otherRoomData.boundsRight, doorTiles);
        }
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
