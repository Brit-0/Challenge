using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTabUIManager : MonoBehaviour
{
    [SerializeField] GameObject craftingTab;
    [SerializeField] Button openCraftButton;
    [SerializeField] UITweener uiTweener;

    public void ToggleCraftingTab()
    {
        if (craftingTab.gameObject.activeInHierarchy)
        {
            uiTweener.CloseTween("crafting");
        }
        else
        {
            craftingTab.transform.localScale = Vector3.zero;
            TowerManager.isTabOpened = true;
            craftingTab.gameObject.SetActive(true);
            openCraftButton.gameObject.SetActive(false);
            uiTweener.PopUpTween();
        }
    }

    public void CloseTab()
    {
        TowerManager.isTabOpened = false;
        craftingTab.gameObject.SetActive(false);
        openCraftButton.gameObject.SetActive(true);
    }
}
