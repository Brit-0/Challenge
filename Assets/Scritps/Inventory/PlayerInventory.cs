using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory current;
 
    public int[] ownedMaterials = new int[3]; // Pedra = 0 / Madeira = 1 / Osso = 2;

    //Lista de torres possuídas
    private Dictionary<TowerData, InventoryTower> towerDictionary;
    public List<InventoryTower> ownedTowers { get; private set; }

    [SerializeField] private TextMeshProUGUI craftingFB;

    private void Awake()
    {
        current = this;
        ownedTowers = new List<InventoryTower>();
        towerDictionary = new Dictionary<TowerData, InventoryTower>();
    }

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

        InventoryUIManager.current.UpdateInventory();
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

        InventoryUIManager.current.UpdateInventory();
    }


    //SISTEMA DE CRAFTING
    public void Craft(TowerData tower)
    {
        //CHECAR SE POSSUI AS PARTES NECESSÁRIAS
        if (!CheckParts(tower.recipe)) 
        {
            craftingFB.text = "Não possui as partes necessárias";
            return;
        }

        //REMOVER PARTES DO INVENTÁRIO
        int index = 0;

        foreach (char number in tower.recipe)
        {
            int needed = number - '0';

            ownedMaterials[index] -= needed;
            InventoryUIManager.current.UpdateLabels();

            index++;
        }

        //ADICIONAR TORRE AO INVENTÁRIO
        current.AddTower(tower);

        //FEEDBACK DE SUCESSO
        craftingFB.text = tower.towerName + " criada com sucesso!";

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


}
