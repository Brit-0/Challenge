using System.Collections;
using UnityEngine;

public class TorreDeOssos : TowerLogic
{

    protected override IEnumerator ActiveLoop()
    {
        return base.ActiveLoop();
    }

    protected override void Shoot()
    {
        foreach (Transform point in activeShootPoints)
        {
            GameObject projectile = Instantiate(towerData.projectilePf, point.position, Quaternion.identity);
            projectile.GetComponent<ProjectileLogic>().SetData(towerData, closestEnemy, towerData.projType, towerData.projEffects);
        }

        animator.SetTrigger("Shoot");
        AudioManager.main.PlaySpatialSound(AudioManager.main.shoot, gameObject);
    }
}
