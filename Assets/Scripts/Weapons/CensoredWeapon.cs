using Hassus.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CensoredWeapon : SpawnedWeaponBase
{
    public int Damage;
    [SerializeField] private GameObject censorBar;
    [SerializeField] private List<AudioClip> collisionSounds;
    
    [SerializeField] private List<GameObject> collisionParticles;

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
        MapLoader.Instance.Explode(transform.position, 1);
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

    private void OnDestroy()
    {
        MapLoader.Instance.Explode(transform.position, 5);
        SFXManager.Instance.PlayOneShot(SFXType.ExplosionRandom);
    }
}
