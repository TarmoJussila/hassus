using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    private TextMeshProUGUI _countdownText;

    // Start is called before the first frame update
    void Start()
    {
        _countdownText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _countdownText.text = "Get ready!\n" + ((int)GameSettings.Instance.Countdown - (int)GameStateSystem.Instance.StateTime);
    }
}
