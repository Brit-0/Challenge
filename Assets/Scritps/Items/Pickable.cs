using System.Collections;
using UnityEngine;

public class Pickable : MonoBehaviour
{
    [SerializeField] private int index;
    public bool hasBeenPickedUp;
    private bool canBePickedUp;

    private void Start()
    {
        StartCoroutine(EnablePickup());
    }

    private IEnumerator EnablePickup()
    {
        yield return new WaitForSeconds(1f);
        canBePickedUp = true;
    }

    public void Pickup(Transform playerTransform)
    {
        if (!canBePickedUp) return;

        hasBeenPickedUp = true;

        var seq = LeanTween.sequence();
        seq.append(LeanTween.move(gameObject, playerTransform, .3f).setEaseInBack());
        seq.append(() => { Destroy(gameObject); });
        seq.append(() => { PlayerInventory.current.AddMaterial(index, 1); });
    }

}
