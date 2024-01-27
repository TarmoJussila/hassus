using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public class PlayerData
    {
        public string Name;
        public int Score = 0;
        public GameObject PlayerObject;
    }

    public Dictionary<int, PlayerData> PlayerDatas = new();
    [SerializeField] private PlayerInputManager playerInputManager;

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
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"Player left: {playerInput.playerIndex}");
    }
}
