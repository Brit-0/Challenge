using UnityEngine;

public class Altar : Interactable
{
    private void Awake()
    {
        tip = "Aperte \"E\" para ativar a relíquia";
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Interact()
    {
        TipsUIManager.current.DisableTip();
        GameManager.main.StartCoroutine(GameManager.main.StartDefensePhase());
    }
}
