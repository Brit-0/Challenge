using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController main;

    [SerializeField] private Transform finalRoomTransform;
    public Vector3 finalRoomPos;

    private void Awake()
    {
        main = this;
        finalRoomPos = finalRoomTransform.position;
    }
}
