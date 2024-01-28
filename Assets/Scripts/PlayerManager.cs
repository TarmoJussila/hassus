using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    /// <summary>
    /// index, change, new amount
    /// </summary>
    public static event Action<int, int, int> OnPlayerScoreChanged;

    public static event Action<PlayerInput> OnPlayerJoin;
    public static event Action<PlayerInput> OnPlayerLeave;

    public class PlayerData
    {
        public string Name;
        public int Score = 0;
        public GameObject PlayerObject;
        public bool UsingController = false;
        public int ControllerId = -1;
        public string KeyboardSchemaName = "";
    }

    public Dictionary<int, PlayerData> PlayerDatas = new();
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private GameObject playerPrefab;

    public void ChangeState(GameState state)
    {
        if (state == GameState.WAITING_FOR_PLAYERS)
        {
            if (!playerInputManager.joiningEnabled)
            {
                Debug.Log("PlayerInputManager joining enabled");
                playerInputManager.EnableJoining();
            }
        }
        else if (playerInputManager.joiningEnabled)
        {
            Debug.Log("PlayerInputManager joining disabled");
            playerInputManager.DisableJoining();
        }
    }

    protected override void OnAwake()
    {
        PlayerHealth.OnDamageDealt += OnDamageDealt;
    }

    private void OnDestroy()
    {
        PlayerHealth.OnDamageDealt -= OnDamageDealt;
    }

    private void OnDamageDealt(int index, int sourceIndex, int amount, bool kill)
    {
        if (sourceIndex > 0)
        {
            GivePlayerScore(sourceIndex, amount + (kill ? 10 : 0));
        }

        GivePlayerScore(index, kill ? -5 : 0);
    }

    public void SetPlayerScore(int index, int score)
    {
        PlayerDatas[index].Score = score;

        OnPlayerScoreChanged?.Invoke(index, score, score);
    }

    private void GivePlayerScore(int index, int score)
    {
        PlayerDatas[index].Score += score;

        OnPlayerScoreChanged?.Invoke(index, score, PlayerDatas[index].Score);
    }

    public void JoinKeyboard(int keyboardSchemeIndex)
    {
        // TODO: link to schemes
        List<string> schemeNames = new List<string>()
        {
            "Keyboard&Mouse",
            "Keyboard arrows",
            "Keyboard TFGH",
            "Keyboard IJKL"
        };

        if (keyboardSchemeIndex >= schemeNames.Count)
        {
            Debug.LogWarning($"Failed to join, invalid keyboard scheme ID: {keyboardSchemeIndex}");
            return;
        }

        string schema = schemeNames[keyboardSchemeIndex];
        if (PlayerDatas.Any(pair => pair.Value.KeyboardSchemaName.Equals(schema)))
        {
            Debug.LogWarning($"Skipped adding player, keyboard schema '{schema}' has already joined");
            return;
        }

        Debug.Log($"Join input from keyboard scheme ID {keyboardSchemeIndex} = {schema} | current player = {PlayerInput.all.Count}");
        int nextId = PlayerInput.all.Count;
        PlayerDatas[nextId] = new PlayerData
        {
            Name = PlayerName(nextId), KeyboardSchemaName = schema, UsingController = false
        };
        PlayerInput.Instantiate(playerPrefab, controlScheme: schema, pairWithDevice: Keyboard.current);
    }

    public void JoinGamePad(Gamepad gamepad)
    {
        if (PlayerDatas.Any(pair => pair.Value.UsingController && pair.Value.ControllerId == gamepad.deviceId))
        {
            Debug.LogWarning($"Skipped adding player, controller id '{gamepad.deviceId}' has already joined");
            return;
        }

        Debug.Log($"Join input from gamepad ID {gamepad.deviceId} = {gamepad.name} | current player = {PlayerInput.all.Count}");
        int nextId = PlayerInput.all.Count;
        PlayerDatas[nextId] = new PlayerData
        {
            Name = PlayerName(nextId), UsingController = true, ControllerId = gamepad.deviceId
        };
        PlayerInput.Instantiate(playerPrefab, controlScheme: "Gamepad", pairWithDevice: gamepad);
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        int id = playerInput.playerIndex;
        if (id >= PlayerDatas.Count)
        {
            Debug.LogWarning($"PlayerData missing for id {id}");
            return;
        }

        Debug.Log($"Player joined: {id}");
        PlayerDatas[id].PlayerObject = playerInput.gameObject;

        // TODO: move player to spawn point
        //playerInput.transform.position = ???

        OnPlayerJoin?.Invoke(playerInput);
        SetPlayerScore(id, 0);
    }

    private static string PlayerName(int id)
    {
        return "Clown" + id;
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"Player left: {playerInput.playerIndex}");

        OnPlayerLeave?.Invoke(playerInput);
    }
}
