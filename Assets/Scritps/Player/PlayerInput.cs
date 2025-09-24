using DG.Tweening;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;

    private float shootCooldown = 1.5f, nextReadyTime;
    public static bool isHealing;

    public bool blockInput;

    private void Update()
    {
        if (blockInput) return;

        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time >= nextReadyTime)
            {
                SkelLogic.main.Shoot();
                nextReadyTime = Time.time + shootCooldown;
            }
        }
        
        if (Input.GetButtonDown("Cancel")) //PAUSE
        {
            pauseMenu.OnClick();
        }
        
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Space)) //SPAWNAR INIMIGOS
        {
            GameManager.main.StartCoroutine(GameManager.main.StartDefensePhase());
        }
        //HEAL
        if (Input.GetKeyDown(KeyCode.Q) && PlayerInventory.current.bandages > 0)
        {
            PlayerCombat.main.healingCoroutine = PlayerCombat.main.StartCoroutine(PlayerCombat.main.StartHealing());
            isHealing = true;
        }

        if (isHealing && Input.GetKeyUp(KeyCode.Q))
        {
            PlayerCombat.main.ResetHealing();
        }
    }

}
