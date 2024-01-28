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
        if (health.TakeDamage(_damage, OwnerPlayer.playerIndex))
        {
            SFXManager.Instance.PlayOneShot(SFXType.Bruh);
        }

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        GetComponent<AudioSource>().Play();

        if (_killParticle != null)
        {
            Instantiate(_killParticle, transform.position, Quaternion.identity);
        }
    }

    protected override void OnCollideOther()
    {
        Destroy(gameObject);
    }
}
