using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creampie : SpawnedWeaponBase
{
    [SerializeField] private int _damage;
    [SerializeField] private GameObject _killParticle;

    protected override void OnHitEnemy(Collider2D other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        health.TakeDamage(_damage, OwnerPlayer.playerIndex);

        if (health.CurrentHealth <= 0)
        {
            Instantiate(_killParticle, health.transform.position, Quaternion.identity);
        }

        GetComponent<AudioSource>().Play();
    }
}
