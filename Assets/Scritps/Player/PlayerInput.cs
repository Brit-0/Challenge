using DG.Tweening;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PauseMenu pauseMenu;

    public static float shootCooldown = 1.5f;
    private float nextReadyTime;
    public static bool isHealing, isSearching;
    public static Searchable searchingItem;

    public static bool blockInput;
    private bool isFlickering;
    [SerializeField] private TowerData towerData;

    private void Update()
    {
        if (isHealing && Input.GetKeyUp(KeyCode.Q))
        {
            PlayerCombat.main.ResetHealing();
        }

        if (blockInput) return;

        //SHOOT
        if (Input.GetMouseButtonDown(1))
        {
            if (Time.time >= nextReadyTime)
            {
                SkelLogic.main.Shoot();
                nextReadyTime = Time.time + shootCooldown;
            }
        }
        
        //PAUSE
        if (Input.GetButtonDown("Cancel")) //PAUSE
        {
            pauseMenu.TogglePause();
        }

        //HEAL
        if (Input.GetKeyDown(KeyCode.Q))
        {
            BandageButton();
        }

        //PLACE TOWER
        if (Input.GetKeyDown(KeyCode.R))
        {
            TowerButton();
        }
    }

    public void BandageButton()
    {
        if (PlayerInventory.current.bandages > 0)
        {
            PlayerCombat.main.healingCoroutine = PlayerCombat.main.StartCoroutine(PlayerCombat.main.StartHealing());
            isHealing = true;
        }
        else if (!isFlickering)
        {
            isFlickering = true;
            PlayerCombat.main.healItemBG.DOColor(new Color32(150, 150, 150, 90), .1f).SetLoops(4, LoopType.Yoyo).OnComplete(() => { isFlickering = false; });
        }
    }

    public void TowerButton()
    {
        if (TowerManager.placeMode)
        {
            TowerManager.current.ExitPlaceMode();
            return;
        }

        if (PlayerInventory.current.Craft(towerData))
        {
            TowerManager.current.EnterPlaceMode(PlayerInventory.current.ownedTowers[0]);
        }
        else if (!isFlickering)
        {
            isFlickering = true;
            PlayerInventory.current.towerItemBG.DOColor(new Color32(150, 150, 150, 90), .1f).SetLoops(4, LoopType.Yoyo).OnComplete(() => { isFlickering = false; });
        }
    }

}
