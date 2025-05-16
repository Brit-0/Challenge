using UnityEngine;


[CreateAssetMenu(fileName = "TowerData_", menuName = "ScriptableObjects/Create Tower", order = 1)]
public class TowerData : ScriptableObject
{
    [Header("GENERAL INFO")]
    public string towerName;
    [Tooltip("Pedra / Madeira / Osso")] public string recipe;
    public string upgradeMaterial;
    public int upgradeCost;
    [Header("STATS")]
    public int damage;
    public int maxHealth;
    public int maxLevel;
    public float detectionRadius;
    public float shootCooldown;
    [Header("PROJECTILE SETTINGS")]
    public float projectileSpeed;
    public ProjetileType projType;
    public ProjectileEffect[] projEffects;

    [Header("ASSETS")]
    public Sprite icon;
    public GameObject towerPf;
    public GameObject projectilePf;

}
