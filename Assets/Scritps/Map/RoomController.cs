using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController main;

    public Vector3 finalRoomPos;

    private void Awake()
    {
        main = this;
    }
}
