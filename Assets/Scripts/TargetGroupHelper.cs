using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetGroupHelper : MonoBehaviour
{
    private CinemachineTargetGroup _group;
    
    private void Awake()
    {
        PlayerManager.OnPlayerJoin += OnPlayerJoin;
        PlayerManager.OnPlayerLeave += OnPlayerLeave;

        _group = GetComponent<CinemachineTargetGroup>();
    }

    private void OnPlayerJoin(PlayerInput player)
    {
        _group.AddMember(player.transform, 1, 3);
    }

    private void OnPlayerLeave(PlayerInput player)
    {
        _group.RemoveMember(player.transform);
    }

    private void OnDestroy()
    {
        PlayerManager.OnPlayerJoin -= OnPlayerJoin;
        PlayerManager.OnPlayerLeave -= OnPlayerLeave;
    }
}
