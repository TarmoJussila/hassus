using System;
using System.Collections;
using System.Collections.Generic;
using Hassus.Map;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponManager : MonoSingleton<WeaponManager>
{
    [SerializeField] private GameObject _spawnerPrefab;

    public List<WeaponDef> WeaponDefs;

    protected override void OnAwake()
    {
        GameStateSystem.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameStateSystem.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.FIGHT)
        {
            foreach (Vector3 point in MapLoader.Instance.ItemPoints)
            {
                Instantiate(_spawnerPrefab, point, Quaternion.identity);
            }
        }
    }

    public WeaponDef GetRandomWeapon()
    {
        return WeaponDefs[Random.Range(0, WeaponDefs.Count)];
    }
}
