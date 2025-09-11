using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private int index;
    public bool hasBeenPickedUp;

    public void Pickup(Transform playerTransform)
    {
        hasBeenPickedUp = true;

        var seq = LeanTween.sequence();
        seq.append(LeanTween.move(gameObject, playerTransform, .3f).setEaseInBack());
        seq.append(() => { Destroy(gameObject); });
        seq.append(() => { PlayerInventory.current.AddMaterial(index, 1); });
    }

}
