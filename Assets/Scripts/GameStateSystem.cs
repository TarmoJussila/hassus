using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public enum GameState
{
    INTRO,
    WAITING_FOR_PLAYERS,
    COUNTDOWN,
    FIGHT,
    GAME_OVER,
    OUTRO
}

[Serializable]
public class StateObjectPair
{
    public GameState State;
    public List<GameObject> StateObjects;
}

public class GameStateSystem : MonoSingleton<GameStateSystem>
{
    [FormerlySerializedAs("_stateObjects")] [SerializeField]
    private List<StateObjectPair> _states = new List<StateObjectPair>();

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

        foreach (StateObjectPair pair in _states)
        {
            foreach (GameObject go in pair.StateObjects)
            {
                go.SetActive(false);
            }
        }

        foreach (StateObjectPair pair in _states)
        {
            foreach (GameObject go in pair.StateObjects)
            {
                if (pair.State == state)
                {
                    go.SetActive(true);
                }
            }
        }

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.ChangeState(state);
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
        if (StateTime > 1f && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.INTRO);
        }
    }

    private void Update_Intro()
    {
        if (StateTime > 1f && Input.GetKeyDown(KeyCode.Return))
        {
            ChangeGameState(GameState.WAITING_FOR_PLAYERS);
        }
    }

    private void Update_GameOver()
    {
        if (StateTime > 1f && Input.GetKeyDown(KeyCode.Return))
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
        // TODO: link to input schemes
        var joinKeys = new List<List<KeyCode>> {
            new() { KeyCode.Q },
            new() { KeyCode.RightControl, KeyCode.RightAlt, KeyCode.Minus },
            new(){ KeyCode.R },
            new() { KeyCode.U }
        };

        for (int i = 0; i < joinKeys.Count; i++)
        {
            for (int j = 0; j < joinKeys[i].Count; j++)
            {
                if (Input.GetKeyDown(joinKeys[i][j]))
                {
                    PlayerManager.Instance.JoinKeyboard(i);
                    break;
                }
            }
        }
        
        foreach (var gamepad in Gamepad.all)
        {
            // TODO: replace hard coded join button with on from schemes
            if (gamepad.aButton.wasPressedThisFrame)
            {
                PlayerManager.Instance.JoinGamePad(gamepad);
            }
        }
    }

    private void Update_Fight()
    {
        if (StateTime > GameSettings.Instance.RoundTime)
        {
            ChangeGameState(GameState.GAME_OVER);
        }
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if (Instance.CurrentState != GameState.WAITING_FOR_PLAYERS)
        {
            Debug.Log($"Start game input ignored on state: {Instance.CurrentState}");
            return;
        }

        if (Instance.StateTime < 2f)
        {
            Debug.Log($"Start game input ignored: StateTime too low");
            return;
        }

        // TODO: check for number of players
        Debug.Log("Start Game input accepted!");
        Instance.ChangeGameState(GameState.COUNTDOWN);
    }
}
