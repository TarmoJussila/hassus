using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BestPlayer : MonoBehaviour
{
    [SerializeField] private List<Sprite> _avatars;

    private void OnEnable()
    {
        var winner = PlayerManager.Instance.PlayerDatas.OrderByDescending(p => p.Value.Score).ToList()[0];
        GetComponent<Image>().sprite = _avatars[winner.Key];
    }
}
