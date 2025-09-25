using UnityEngine;

public static class StaticReset
{
    public static void ResetAll()
    {
        PlayerMovement.canMove = true;
        PlayerInput.blockInput = false;
        GameManager.currentGamePhase = GamePhase.Exploration;
    }
}
