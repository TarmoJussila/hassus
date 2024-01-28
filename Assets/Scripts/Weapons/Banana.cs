using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : SpawnedWeaponBase
{
    [SerializeField] private int _damage;
    [SerializeField] private GameObject _killParticle;

    protected override void OnStart()
    {
        SFXManager.Instance.PlayOneShot(SFXType.Wobble);
    }

    protected override void OnCollideEnemy(Collision2D coll)
    {
        PlayerHealth health = coll.gameObject.GetComponent<PlayerHealth>();

        if (health.CurrentHealth <= 0) return;

        health.TakeDamage(_damage, OwnerPlayer.playerIndex);

        Destroy(gameObject);
        SFXManager.Instance.PlayOneShot(SFXType.Vineboom);
        SFXManager.Instance.PlayOneShot(SFXType.Wobble);
    }

    protected override void OnCollideSelf()
    {
        PlayerHealth health = OwnerPlayer.gameObject.GetComponent<PlayerHealth>();

        if (health.CurrentHealth <= 0) return;
        health.TakeDamage(_damage, OwnerPlayer.playerIndex);

        Destroy(gameObject);
        SFXManager.Instance.PlayOneShot(SFXType.Vineboom);
        SFXManager.Instance.PlayOneShot(SFXType.Wobble);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        if (_killParticle != null)
        {
            Instantiate(_killParticle, transform.position, Quaternion.identity);
        }
    }
}
