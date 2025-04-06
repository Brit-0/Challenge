using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "TowerData_", menuName = "ScriptableObjects/Create Tower", order = 1)]
public class TowerData : ScriptableObject
{
    public string towerName;
    public string recipe;
    public float attackRange;
    public float attackSpeed;
    public int towerDamage;
    public int maxHealth;

    public Sprite icon;
    public Color32 towerColor;
    public GameObject towerPf;
    private int currentHealth;

}
