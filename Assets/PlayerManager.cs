using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    /// <summary>
    /// index, change, new amount
    /// </summary>
    public static event Action<int, int, int> OnPlayerScoreChanged;

    public static event Action<int> OnPlayerJoin;
    public static event Action<int> OnPlayerLeave;

    public class PlayerData
    {
        public string Name;
        public int Score = 0;
        public GameObject PlayerObject;
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
        GivePlayerScore(sourceIndex, amount + (kill ? 10 : 0));
        GivePlayerScore(index, kill ? -5 : 0);
    }

    public void GivePlayerScore(int index, int score)
    {
        PlayerDatas[index].Score += score;

        OnPlayerScoreChanged?.Invoke(index, score, PlayerDatas[index].Score);
    }

    public void OnJoinInput(int keyboardSchemeIndex)
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

        string scheme = schemeNames[keyboardSchemeIndex];
        Debug.Log($"Join input from keyboard scheme ID {keyboardSchemeIndex} = {scheme}");
        PlayerInput.Instantiate(playerPrefab, controlScheme: scheme, pairWithDevice: Keyboard.current);
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

        PlayerDatas[playerInput.playerIndex] = new PlayerData()
        {
            Name = "Clown" + playerInput.playerIndex, PlayerObject = playerInput.gameObject
        };

        Debug.Log($"Player joined: {playerInput.playerIndex}");
        // TODO: move player to spawn point
        //playerInput.transform.position = ???

        OnPlayerJoin?.Invoke(playerInput.playerIndex);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"Player left: {playerInput.playerIndex}");
        
        OnPlayerLeave?.Invoke(playerInput.playerIndex);
    }
}
