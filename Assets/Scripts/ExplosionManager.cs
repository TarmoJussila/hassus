using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoSingleton<ExplosionManager>
{
    [SerializeField] private GameObject _explosionPrefab;

    public Explosion SpawnExplosion(Vector3 position)
    {
        return Instantiate(_explosionPrefab, position, Quaternion.identity).GetComponent<Explosion>();
    }
}
