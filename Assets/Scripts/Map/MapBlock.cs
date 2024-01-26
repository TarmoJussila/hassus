using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] foliageObjects;
    [SerializeField] private GameObject grassObject;

    public void ToggleGrass(bool toggle)
    {
        grassObject.SetActive(toggle);
    }

    public void ToggleFoliage(float chance, int index = -1)
    {
        bool enableFoliage = chance > Random.Range(0.0f, 1.0f);
        int randomIndex = index == -1 ? Random.Range(0, foliageObjects.Length) : index;
        for (int i = 0; i < foliageObjects.Length; i++)
        {
            foliageObjects[i].SetActive(i == randomIndex && enableFoliage);
        }
    }
}
