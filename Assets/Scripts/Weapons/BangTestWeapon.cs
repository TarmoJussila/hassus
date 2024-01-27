using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangTestWeapon : SpawnedWeaponBase
{
    public int Damage;

    protected override void OnStayEnemy(Collider2D other)
    {
        other.GetComponent<PlayerHealth>().TakeDamage(Damage, OwnerPlayer.playerIndex);
    }

    protected override void OnStaySelf()
    {
        OwnerPlayer.GetComponent<PlayerHealth>().TakeDamage(Damage, OwnerPlayer.playerIndex);
    }
}
