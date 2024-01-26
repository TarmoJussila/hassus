using System;
using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject entityToSpawn;
    [SerializeField] private WeaponSO[] weaponSOs;
    [SerializeField] private float spawnDelaySeconds = 1;
    [SerializeField] private SpriteRenderer weaponSprite;

    BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        SpawnRandomWeapon();
    }

    private void Update()
    {
        CheckCollisions();
    }

    void CheckCollisions()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(boxCollider.bounds.center, boxCollider.bounds.size, 0f);

        foreach (Collider2D collider in colliders)
        {
            // TODO: Check if collided with player
            if (false)
            {
                weaponSprite.enabled = false;
                // call player's PickWeapon() or something
                StartCoroutine(DelayedWeaponSpawn());
            }
        }
    }

    private IEnumerator DelayedWeaponSpawn()
    {
        yield return new WaitForSeconds(spawnDelaySeconds);
        SpawnRandomWeapon();
    }

    private void SpawnRandomWeapon()
    {
        WeaponSO randomWeaponSO = weaponSOs[UnityEngine.Random.Range(0, weaponSOs.Length)];
        weaponSprite.sprite = randomWeaponSO.sprite;
        weaponSprite.enabled = true;
    }
}