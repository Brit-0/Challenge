using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class CraftingTabUIManager : MonoBehaviour
{
    private bool clicking, reverting, closing;
    [SerializeField] private RectTransform craftingTab;
    [SerializeField] private Button openCraftButton;
    private RectTransform button;
    private Vector2 buttonNewScale, buttonOldScale;
    private Vector2 tabNewScale, tabOldScale;
    

    public void ToggleCraftingTab()
    {
        if (craftingTab.gameObject.activeInHierarchy)
        {
            TowerManager.isTabOpened = false;
            craftingTab.gameObject.SetActive(false);
            openCraftButton.gameObject.SetActive(true);
            //CloseAnimation();
        }
        else
        {
            TowerManager.isTabOpened = true;
            craftingTab.gameObject.SetActive(true);
            openCraftButton.gameObject.SetActive(false);
        }
    }

    private void CloseAnimation()
    {
        if (closing) return;

        tabOldScale = craftingTab.sizeDelta;
        tabNewScale = craftingTab.sizeDelta - craftingTab.sizeDelta;
        closing = true;
    }

    public void ClickAnimation(RectTransform button)
    {
        this.button = button;

        if (clicking || reverting)
        {
            Reset();
        }

        buttonOldScale = button.sizeDelta;
        buttonNewScale = new Vector2(button.rect.width - 30, button.rect.height - 20);

        clicking = true;
    }

    private void Reset()
    {
        clicking = false;
        reverting = false;
        button.sizeDelta = buttonOldScale;
    }



    private void Update()
    {
        //BOTÕES DE CRAFT
        if (clicking)
        {
            button.sizeDelta = Vector2.Lerp(button.sizeDelta, buttonNewScale, 20 * Time.deltaTime);

            if (Vector2.Distance(button.sizeDelta, buttonNewScale) < 1)
            {
                clicking = false;
                reverting = true;
            }
        }
        else if (reverting)
        {
            button.sizeDelta = Vector2.Lerp(button.sizeDelta, buttonOldScale, 15 * Time.deltaTime);

            if (Vector2.Distance(button.sizeDelta, buttonOldScale) < .1f)
            {
                reverting = false;
            }
        }

        //ABA DE CRAFTING
        /*if (closing)
        {
            craftingTab.sizeDelta = Vector2.Lerp(craftingTab.sizeDelta, craftingTab.rect.size, 9 * Time.deltaTime);

            if (Vector2.Distance(craftingTab.sizeDelta, craftingTab.rect.size) < 1f)
            {
                closing = false;

                craftingTab.gameObject.SetActive(false);
                craftingTab.sizeDelta = tabOldScale;
                openCraftButton.gameObject.SetActive(true);
            }
        }
        */
        
    }
}
