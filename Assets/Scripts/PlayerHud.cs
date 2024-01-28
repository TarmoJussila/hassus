using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private Image _aliveFace;
    [SerializeField] private Image _deadFace;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _playerIndex;

    private void Awake()
    {
        _playerIndex = transform.GetSiblingIndex();

        PlayerManager.OnPlayerScoreChanged += OnPlayerScoreChanged;
        PlayerHealth.OnPlayerHealthChanged += OnPlayerHealthChanged;
        PlayerHealth.OnPlayerDead += OnPlayerDead;
        PlayerHealth.OnPlayerRespawn += OnPlayerRespawn;
    }

    private void OnDestroy()
    {
        PlayerManager.OnPlayerScoreChanged -= OnPlayerScoreChanged;
        PlayerHealth.OnPlayerHealthChanged -= OnPlayerHealthChanged;
        PlayerHealth.OnPlayerDead -= OnPlayerDead;
        PlayerHealth.OnPlayerRespawn -= OnPlayerRespawn;
    }

    private void OnPlayerRespawn(int index, GameObject go)
    {
        if (index != _playerIndex) { return; }

        _aliveFace.gameObject.SetActive(true);
        _deadFace.gameObject.SetActive(false);
    }

    private void OnPlayerDead(int index, GameObject go)
    {
        if (index != _playerIndex) { return; }

        _aliveFace.gameObject.SetActive(false);
        _deadFace.gameObject.SetActive(true);

        _healthText.text = "DEAD!";
    }

    private void OnPlayerHealthChanged(int index, int oldh, int newh, int maxh)
    {
        if (index != _playerIndex) { return; }

        _healthText.text = "Health: " + newh;
    }

    private void OnPlayerScoreChanged(int index, int change, int newAmount)
    {
        if (index != _playerIndex) { return; }

        _scoreText.text = "Score: " + newAmount;
    }
}
