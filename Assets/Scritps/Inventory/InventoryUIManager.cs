using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager current;

    //REFERÊNCIAS UI
    [SerializeField] private TextMeshProUGUI pedraLbl, madeiraLbl, ossoLbl;
    [SerializeField] private GameObject slotPf;
    [SerializeField] private GameObject towerInventoryBar;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //UpdateLabels();
    }

    public void UpdateLabels()
    {
        pedraLbl.text = "Pedras: " + PlayerInventory.current.ownedMaterials[0];
        madeiraLbl.text = "Madeiras: " + PlayerInventory.current.ownedMaterials[1];
        ossoLbl.text = "Ossos: " + PlayerInventory.current.ownedMaterials[2];
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
