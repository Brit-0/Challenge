using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TowerData_", menuName = "ScriptableObjects/Create Tower", order = 1)]
public class TowerData : ScriptableObject
{
    public string towerName;
    public string recipe;
    public string upgradeMaterial;
    public int upgradeCost;
    public float attackRange;
    public float attackSpeed;
    public int towerDamage;
    public int maxHealth;

    public Sprite icon;
    public GameObject towerPf;

}
