using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangTestWeapon : SpawnedWeaponBase
{
    public int Damage;
    [SerializeField] private GameObject _killParticle;

    protected override void OnStayEnemy(Collider2D other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        DealDamage(health);
    }

    protected override void OnStaySelf()
    {
        PlayerHealth health = OwnerPlayer.GetComponent<PlayerHealth>();
        DealDamage(health);
    }

    private void DealDamage(PlayerHealth health)
    {
        if (health.TakeDamage(Damage, OwnerPlayer.playerIndex))
        {
            Instantiate(_killParticle, health.transform.position, Quaternion.identity);
        }
    }
}
