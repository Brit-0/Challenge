using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Start,
    Common,
    Trap
}

public class Room : MonoBehaviour
{
    [SerializeField] private string roomName;
    [SerializeField] private RoomType type;

    [Header("ÁREA DAS PORTAS")]
    public BoundsInt boundsDown;
    public BoundsInt boundsLeft;
    public BoundsInt boundsUp;
    public BoundsInt boundsRight;

    [SerializeField] private List<string> availableDirections = new List<string>();
    private bool alreadySet = false;

    public List<string> GetDirections()
    {
        if (alreadySet)
        {
            return availableDirections;
        }
        else
        {
            print("setou");
            availableDirections.Clear();
            availableDirections.Add("Down");
            availableDirections.Add("Left");
            availableDirections.Add("Up");
            availableDirections.Add("Right");

            alreadySet = true;

            return availableDirections;
        }
        
    }

    public void RemoveDirection(string dir)
    {
        availableDirections.Remove(dir);
    }
}
