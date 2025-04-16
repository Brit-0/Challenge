using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class TowerCard : MonoBehaviour
{

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI nameLabel;
    [SerializeField] GameObject stackObj;
    [SerializeField] TextMeshProUGUI stackLabel;
    [SerializeField] InventoryTower cardTower;
    public void Set(InventoryTower tower)
    {
        cardTower = tower;

        icon.sprite = tower.data.icon;
        nameLabel.text = tower.data.towerName;
        if (tower.stackSize <= 1)
        {
            stackObj.SetActive(false);
            return;
        }

        stackLabel.text = tower.stackSize.ToString();
    }

    public void CardClick()
    {
        if (!TowerManager.placeMode)
        {
            TowerManager.current.EnterPlaceMode(cardTower, gameObject);
        }
        else
        {
            TowerManager.current.ExitPlaceMode();
        }
    }
}
