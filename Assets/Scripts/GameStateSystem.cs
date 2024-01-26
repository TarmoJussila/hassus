using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    INTRO,
    WAITING_FOR_PLAYERS,
    COUNTDOWN,
    FIGHT,
    GAME_OVER,
    OUTRO
}

public class GameStateSystem : MonoSingleton<GameStateSystem>
{
    public GameState CurrentState { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    public float StateTime { get; private set; }

    public void ChangeGameState(GameState state)
    {
        StateTime = 0f;
        CurrentState = state;
        OnGameStateChanged?.Invoke(state);
    }

    private void Update()
    {
        StateTime += Time.deltaTime;

        switch (CurrentState)
        {
            case GameState.WAITING_FOR_PLAYERS:
            {
                Update_WaitingPlayers();
                break;
            }
            case GameState.FIGHT:
            {
                Update_Fight();
                break;
            }
        }
    }

    private void Update_WaitingPlayers()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.COUNTDOWN);
        }
    }

    private void Update_Fight()
    {
        if (StateTime > GameSettings.Instance.GameTime)
        {
            ChangeGameState(GameState.GAME_OVER);
        }
    }
}
