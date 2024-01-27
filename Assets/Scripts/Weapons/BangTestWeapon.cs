using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BangTestWeapon : SpawnedWeaponBase
{
    public int Damage;
    
    protected override void OnOverlapEnemy(Collider2D other)
    {
        other.GetComponent<PlayerHealth>().TakeDamage(Damage);
        
        base.OnOverlapEnemy(other);
    }
}
