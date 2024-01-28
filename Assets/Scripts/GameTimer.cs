using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private TextMeshProUGUI _timeLeftText;

    void Start()
    {
        _timeLeftText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeftText.text = "Time left: " + ((int)GameSettings.Instance.RoundTime - (int)GameStateSystem.Instance.StateTime);
    }
}
