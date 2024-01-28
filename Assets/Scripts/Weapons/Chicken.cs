using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : SpawnedWeaponBase
{
    [SerializeField] private int _damage;
    [SerializeField] private List<AudioClip> _honks;

    protected override void OnStart()
    {
        GetComponent<AudioSource>().PlayOneShot(_honks[Random.Range(0, _honks.Count)]);
    }

    protected override void OnOverlapEnemy(Collider2D other)
    {
        other.GetComponent<PlayerHealth>().TakeDamage(_damage, OwnerPlayer.playerIndex);
        SFXManager.Instance.PlayOneShot(SFXType.Bonk);
    }
}
