using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;

public class Slime : Interactable
{
    [SerializeField] private GameObject questBaloon;
    [SerializeField] private SpriteRenderer item;
    [SerializeField] private Color baloonColor;

    protected override void Interact()
    {
        ShowQuest();
    }

    private void ShowQuest()
    {
        
    }
}
