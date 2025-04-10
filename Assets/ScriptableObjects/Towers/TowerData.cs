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
    public int towerDamage;
    public int maxHealth;
    public float detectionRadius;
    public float shootCooldown;
    public float projectileSpeed;

    public Sprite icon;
    public GameObject towerPf;
    public GameObject projectilePf;

}
