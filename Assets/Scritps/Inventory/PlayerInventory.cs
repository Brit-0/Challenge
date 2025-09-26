using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory current;
 
    public int[] ownedMaterials = new int[3]; // Pedra = 0 / Madeira = 1 / Osso = 2;

    //Lista de torres possuídas
    private Dictionary<TowerData, InventoryTower> towerDictionary;
    public List<InventoryTower> ownedTowers { get; private set; }

    [SerializeField] private TextMeshProUGUI craftingFB;
    [SerializeField] private LayerMask itemsLayer;
    [SerializeField] private TMP_Text bandagesLbl;
    [SerializeField] private TMP_Text towersLbl;
    public Image towerItemBG;
    private bool isFlashingFB;

    [SerializeField] private TMP_Text pedraLbl, madeiraLbl, ossoLbl;

    public int bandages;

    private void Awake()
    {
        current = this;
        ownedTowers = new List<InventoryTower>();
        towerDictionary = new Dictionary<TowerData, InventoryTower>();
    }

    private void Update()
    {
        CheckForPickup();
    }

    public void ChangeBandages(int amount)
    {
        bandages += amount;
        bandagesLbl.text = bandages.ToString();
    }

    #region MATERIALS

    public void AddMaterial(int index, int amount)
    {
        ownedMaterials[index] += amount;
        UpdateLabels();
    }

    public void RemoveMaterial(int index, int amount)
    {
        ownedMaterials[index] -= amount;
        UpdateLabels();
    }

    public void UpdateLabels()
    {
        pedraLbl.text = ownedMaterials[0].ToString();
        madeiraLbl.text = ownedMaterials[1].ToString();
        ossoLbl.text = ownedMaterials[2].ToString();
    }

    #endregion

    #region CHECK FOR ITEMS

    private void CheckForPickup()
    {
        Collider2D[] nearbyItems = Physics2D.OverlapCircleAll(transform.position, .8f, itemsLayer);

        foreach (Collider2D item in nearbyItems)
        {
            if (item.GetComponent<Pickable>().hasBeenPickedUp) continue;
            item.GetComponent<Pickable>().Pickup(transform);
        }
    }

    #endregion

    #region TOWERS

    public void AddTower(TowerData referenceData)
    {
        if (towerDictionary.TryGetValue(referenceData, out InventoryTower value))
        {
            value.AddToStack();

            //print("Agora possui " + value.stackSize + " da " + referenceData.towerName + "!");
        }
        else
        {
            InventoryTower newTower = new InventoryTower(referenceData);
            ownedTowers.Add(newTower);
            towerDictionary.Add(referenceData, newTower);

            //print("Agora possui a torre " + referenceData.towerName + "!");
        }

        towersLbl.text = ownedTowers.Count.ToString();
    }

    public void RemoveTower(TowerData referenceData)
    {
        if (towerDictionary.TryGetValue(referenceData, out InventoryTower value))
        {
            value.RemoveFromStack();

            if (value.stackSize == 0)
            {
                ownedTowers.Remove(value);
                towerDictionary.Remove(referenceData);
            }
        }

        towersLbl.text = ownedTowers.Count.ToString();
    }

    #endregion

    #region CRAFTING

    public bool Craft(TowerData tower)
    {
        //CHECAR SE POSSUI AS PARTES NECESSÁRIAS
        if (!CheckParts(tower.recipe)) 
        {
            return false;
        }

        //ADICIONAR TORRE AO INVENTÁRIO
        current.AddTower(tower);

        return true;
    }

    private bool CheckParts(string recipeID)
    {
        int index = 0;
        List<int> failedIndexes = new List<int>();

        foreach (char number in recipeID)
        {
            int needed = number - '0';

            if (ownedMaterials[index] < needed)
            {
                failedIndexes.Add(index);
                //print("Não possui o suficiente da parte " + index);
                //print("Possui " + ownedParts[index] + " e precisa de " + needed);
            }

            index++;
        }

        if (failedIndexes.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveMaterials(TowerData towerData)
    {
        int index = 0;

        foreach (char number in towerData.recipe)
        {
            int needed = number - '0';

            RemoveMaterial(index, needed);

            index++;
        }
    }

    #endregion

    #region FEEDBACK

    private IEnumerator FlashFeedback(string message)
    {
        if (isFlashingFB) yield break;

        craftingFB.text = message;
        isFlashingFB = true;

        yield return new WaitForSeconds(2f);

        craftingFB.text = "";
        isFlashingFB = false;
    }

    #endregion
}
