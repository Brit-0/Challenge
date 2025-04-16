using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTabUIManager : MonoBehaviour
{

    [SerializeField] GameObject craftingTab;
    [SerializeField] Button openCraftButton;
    [SerializeField] Animator animator;

    public void ToggleCraftingTab()
    {
        if (craftingTab.gameObject.activeInHierarchy)
        {
            TowerManager.isTabOpened = false;
            animator.SetTrigger("Close");
        }
        else
        {
            TowerManager.isTabOpened = true;
            craftingTab.gameObject.SetActive(true);
            openCraftButton.gameObject.SetActive(false);
            animator.SetTrigger("Open");
        }
    }

    public void CloseTab()
    {
        craftingTab.gameObject.SetActive(false);
        openCraftButton.gameObject.SetActive(true);
    }
}
