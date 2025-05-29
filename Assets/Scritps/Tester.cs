using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] LayerMask roomsLayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask(new string[] { "Rooms" })))
            {
                print("Touching");
            }
            else
            {
                print("Not Touching");
            }
        }
    }
}
