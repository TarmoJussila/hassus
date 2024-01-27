using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grave : MonoBehaviour
{
    [SerializeField] private List<Sprite> _sprites;

    public void SetPlayerIndex(int index)
    {
        GetComponentInChildren<SpriteRenderer>().sprite = _sprites[index];
    }
}
