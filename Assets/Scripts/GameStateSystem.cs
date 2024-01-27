using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    INTRO,
    WAITING_FOR_PLAYERS,
    COUNTDOWN,
    FIGHT,
    GAME_OVER,
    OUTRO
}

[System.Serializable]
public class StateObjectPair
{
    public GameState State;
    public GameObject StateObject;
}

public class GameStateSystem : MonoSingleton<GameStateSystem>
{
    [SerializeField] private List<StateObjectPair> _stateObjects = new List<StateObjectPair>();

    public GameState CurrentState { get; private set; }

    public static event Action<GameState> OnGameStateChanged;

    public float StateTime { get; private set; }

    protected override void OnAwake()
    {
        ChangeGameState(GameState.INTRO);
    }

    public void ChangeGameState(GameState state)
    {
        StateTime = 0f;
        CurrentState = state;

        foreach (StateObjectPair pair in _stateObjects)
        {
            pair.StateObject.SetActive(pair.State == state);
        }

        OnGameStateChanged?.Invoke(state);
    }

    private void Update()
    {
        StateTime += Time.deltaTime;

        switch (CurrentState)
        {
            case GameState.INTRO:
            {
                Update_Intro();
                break;
            }
            case GameState.WAITING_FOR_PLAYERS:
            {
                Update_WaitingPlayers();
                break;
            }
            case GameState.COUNTDOWN:
            {
                Update_Countdown();
                break;
            }
            case GameState.FIGHT:
            {
                Update_Fight();
                break;
            }
            case GameState.GAME_OVER:
            {
                Update_GameOver();
                break;
            }
            case GameState.OUTRO:
            {
                Update_Outro();
                break;
            }
        }
    }

    private void Update_Outro()
    {
        if (StateTime > 2f && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.INTRO);
        }
    }

    private void Update_Intro()
    {
        if (StateTime > 2f && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.WAITING_FOR_PLAYERS);
        }
    }

    private void Update_GameOver()
    {
        if (StateTime > 2f && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.OUTRO);
        }
    }

    private void Update_Countdown()
    {
        if (StateTime > GameSettings.Instance.Countdown)
        {
            ChangeGameState(GameState.FIGHT);
        }
    }

    private void Update_WaitingPlayers()
    {
        // Moved stuff from here to StartGame()
    }

    private void Update_Fight()
    {
        if (StateTime > GameSettings.Instance.RoundTime)
        {
            ChangeGameState(GameState.GAME_OVER);
        }
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        //TODO: only allow joining in one state
        /*
        if (CurrentState != GameState.WAITING_FOR_PLAYERS)
        {
            Debug.Log($"Player failed to join on State: {CurrentState}");
            Destroy(playerInput.gameObject);
            return;
        }
        */
        Debug.Log($"Player joined: {playerInput.playerIndex}");
        // TODO: move player to spawn point
        //playerInput.transform.position = ???
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"Player left: {playerInput.playerIndex}");
        // TODO: delete player
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if (CurrentState != GameState.WAITING_FOR_PLAYERS)
        {
            Debug.Log($"Start game input ignored on state: {CurrentState}");
            return;
        }

        if (StateTime < 2f)
        {
            Debug.Log($"Start game input ignored: StateTime too low");
            return;
        }

        // TODO: check for number of players
        Debug.Log("Start Game input accepted!");
        ChangeGameState(GameState.COUNTDOWN);
    }
}
