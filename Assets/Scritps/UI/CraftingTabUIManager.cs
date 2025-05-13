using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTabUIManager : UIHandler
{
    [SerializeField] GameObject craftingTab;
    [SerializeField] Button openCraftButton;
    private UITweener uiTweener;

    public void OnClick()
    {
        uiTweener = GetComponent<UITweener>();
        ToggleTab(craftingTab, true, .2f, uiTweener, true);
    }
    
    protected override bool ToggleTab(GameObject obj, bool hasPopUp = false, float scale = 0, UITweener uITweener = null, bool blockMovement = false)
    {
        bool opened = base.ToggleTab(obj, hasPopUp, scale, uITweener, blockMovement);

        if (opened)
        {
            openCraftButton.gameObject.SetActive(false);
        }
        else
        {
            openCraftButton.gameObject.SetActive(true);
        }

        return opened;
    }

}
