using UnityEngine;


public class Slime : Interactable
{

    private void Awake()
    {
        tip = "Aperte \"E\" para deixar a slime te morder";
    }

    protected override void Interact()
    {
        PlayerCombat.main.TakeDamage(1);
        AudioManager.main.PlaySound(AudioManager.main.slimeAttack);
    }
}
