using System.Collections;
using System.Collections.Generic;
using Hassus.Map;
using UnityEngine;

public class BangTestWeapon : SpawnedWeaponBase
{
    public int Damage;
    [SerializeField] private GameObject _killParticle;

    protected override void OnStart()
    {
        SFXManager.Instance.PlayOneShot(SFXType.FartRandom);
        MapLoader.Instance.Explode(transform.position, 1);
    }

    protected override void OnStayEnemy(Collider2D other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        DealDamage(health, Damage);
    }

    protected override void OnStaySelf()
    {
        PlayerHealth health = OwnerPlayer.GetComponent<PlayerHealth>();
        DealDamage(health, Mathf.RoundToInt(Damage / 4f));
    }

    private void DealDamage(PlayerHealth health, int damageAmount)
    {
        if (health.TakeDamage(damageAmount, OwnerPlayer.playerIndex))
        {
            SFXManager.Instance.PlayOneShot(SFXType.Vineboom);
            Instantiate(_killParticle, health.transform.position, Quaternion.identity);
        }
    }
}
