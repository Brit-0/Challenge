using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager current;

    //Referências UI
    [SerializeField] private TextMeshProUGUI pedraLbl, madeiraLbl, ossoLbl;
    [SerializeField] private RectTransform craftingTab;
    [SerializeField] private Button openCraftButton;
    [SerializeField] private GameObject slotPf;
    [SerializeField] private GameObject towerInventoryBar;

    private PlayerInventory playerInventory;

    private void Awake()
    {
        current = this;
    }

    //ATUALIZAÇÃO DAS LABELS
    private void Update()
    {
        pedraLbl.text = "Pedras: " + PlayerInventory.current.ownedMaterials[0];
        madeiraLbl.text = "Madeiras: " + PlayerInventory.current.ownedMaterials[1];
        ossoLbl.text = "Ossos: " + PlayerInventory.current.ownedMaterials[2];
    }

    public void ToggleCraftingTab()
    {
        if (craftingTab.gameObject.activeInHierarchy)
        {
            craftingTab.gameObject.SetActive(false);
            openCraftButton.gameObject.SetActive(true);
        }
        else
        {
            craftingTab.gameObject.SetActive(true);
            openCraftButton.gameObject.SetActive(false);
        }
    }

    public void UpdateInventory()
    {
        foreach(Transform t in towerInventoryBar.transform)
        {
            Destroy(t.gameObject);
        }

        DrawInventory();
    }

    private void DrawInventory()
    {
        foreach(InventoryTower tower in PlayerInventory.current.ownedTowers)
        {
            AddTowerSlot(tower);
        }
    }
    private void AddTowerSlot(InventoryTower tower)
    {
        GameObject slot = Instantiate(slotPf);
        slot.transform.SetParent(towerInventoryBar.transform, false);

        TowerCard towerCard = slot.GetComponent<TowerCard>();
        towerCard.Set(tower);
    }
}
