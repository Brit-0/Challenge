using UnityEngine;

public static class StaticReset
{
    public static void ResetAll()
    {
        PlayerMovement.canMove = true;
        PlayerInput.blockInput = false;
        GameManager.currentGamePhase = GamePhase.Exploration;
        FinalScreen.killedEnemies = 0;
        FinalScreen.damageTaken = 0;
        FinalScreen.builtTowers = 0;
        PlayerInput.shootCooldown = 1.5f;
    }
}
