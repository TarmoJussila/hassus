using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHudManager : MonoBehaviour
{
    private List<PlayerHud> _huds;

    void Awake()
    {
        _huds = GetComponentsInChildren<PlayerHud>(true).ToList();
        
        PlayerManager.OnPlayerJoin += OnPlayerJoin;
        PlayerManager.OnPlayerLeave += OnPlayerLeave;
    }

    private void OnDestroy()
    {
        PlayerManager.OnPlayerJoin -= OnPlayerJoin;
        PlayerManager.OnPlayerLeave -= OnPlayerLeave;
    }

    private void OnPlayerLeave(PlayerInput input)
    {
        _huds[input.playerIndex].gameObject.SetActive(false);
    }

    private void OnPlayerJoin(PlayerInput input)
    {
        _huds[input.playerIndex].gameObject.SetActive(true);
    }
}
