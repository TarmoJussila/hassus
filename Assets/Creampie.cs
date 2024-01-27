using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creampie : SpawnedWeaponBase
{
    [SerializeField] private int _damage;
    [SerializeField] private GameObject _killParticle;

    protected override void OnCollideEnemy(Collision2D coll)
    {
        PlayerHealth health = coll.gameObject.GetComponent<PlayerHealth>();
        health.TakeDamage(_damage, OwnerPlayer.playerIndex);

        if (health.CurrentHealth <= 0 && _killParticle != null)
        {
            Instantiate(_killParticle, health.transform.position, Quaternion.identity);
        }

        GetComponent<AudioSource>().Play();
    }
}
