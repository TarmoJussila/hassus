using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoSingleton<CameraController>
{
    protected override void OnAwake()
    {
        GameStateSystem.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state) { }
}
