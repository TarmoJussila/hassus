using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangTestWeapon : SpawnedWeaponBase
{
    public int Damage;

    protected override void OnStart()
    {
        transform.position = OwnerPlayer.transform.position;
    }

    protected override void OnStayEnemy(Collider2D other)
    {
        other.GetComponent<PlayerHealth>().TakeDamage(Damage, OwnerPlayer.playerIndex);
        
        base.OnOverlapEnemy(other);
    }

    protected override void OnStaySelf()
    {
        OwnerPlayer.GetComponent<PlayerHealth>().TakeDamage(Damage, OwnerPlayer.playerIndex);

        base.OnStaySelf();
    }
}
