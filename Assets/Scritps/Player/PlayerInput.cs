using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private GameObject swordPivot;
    [SerializeField] private PauseMenu pauseMenu;

    private float shootCooldown = 1.5f, nextReadyTime;

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
        else if (Input.GetButtonDown("Cancel")) //PAUSE
        {
            pauseMenu.OnClick();
        }
        else if (Input.GetKeyDown(KeyCode.Space)) //SPAWNAR INIMIGOS
        {
            GameManager.main.StartCoroutine(GameManager.main.StartDefensePhase());
        }
    }

}
