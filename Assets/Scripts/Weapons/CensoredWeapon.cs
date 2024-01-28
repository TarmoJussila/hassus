using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CensoredWeapon : SpawnedWeaponBase
{
    public int Damage;
    [SerializeField] private GameObject censorBar;
    [SerializeField] private List<AudioClip> collisionSounds;

    protected override void OnCollideEnemy(Collision2D coll)
    {
        coll.collider.GetComponent<PlayerHealth>().TakeDamage(Damage, OwnerPlayer.playerIndex);
    }

    protected override void OnCollideSelf()
    {
        OwnerPlayer.GetComponent<PlayerHealth>().TakeDamage(Damage, OwnerPlayer.playerIndex);
        PlayCollisionSound();
    }

    protected override void OnCollideOther()
    {
        PlayCollisionSound();
    }

    private void PlayCollisionSound()
    {
        GetComponent<AudioSource>().PlayOneShot(collisionSounds[Random.Range(0, collisionSounds.Count)]);
    }

    private void Update()
    {
        censorBar.transform.rotation = Quaternion.identity;
    }
}
