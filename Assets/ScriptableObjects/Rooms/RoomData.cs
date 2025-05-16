using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum RoomType 
{ 
    Start,
    Common,
    Trap
}

[CreateAssetMenu(fileName = "RoomData_", menuName = "ScriptableObjects/Create Room", order = 1)]
public class RoomData : ScriptableObject
{
    public string roomName;
    public RoomType type;
    public BoundsInt boundsDown, boundsLeft, boundsUp, boundsRight;
}
