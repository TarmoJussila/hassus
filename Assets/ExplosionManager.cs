using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoSingleton<ExplosionManager>
{
    [SerializeField] private GameObject _explosionPrefab;

    public void SpawnExplosion(Vector3 position)
    {
        Instantiate(_explosionPrefab, position, Quaternion.identity);
    }
}
