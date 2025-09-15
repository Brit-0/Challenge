using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePhase
{
    None,
    Exploration,
    Defense
}

public enum GameState
{
    Menu,
    Playing,
    Paused
}

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    public static GamePhase currentGamePhase = GamePhase.Exploration;
    public static GameState currentGameState = GameState.Playing;

    public static Action ChangeGamePhase;

    private void Awake()
    {
        main = this;
    }
}
