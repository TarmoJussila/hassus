using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private void OnPlayerLeave(int index)
    {
        _huds[index].gameObject.SetActive(false);
    }

    private void OnPlayerJoin(int index)
    {
        _huds[index].gameObject.SetActive(true);
    }
}
