using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private WeaponSO[] weaponSOs;
    [SerializeField] private float spawnDelaySeconds = 1;
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private BoxCollider2D pickupCollider;
    
    private WeaponSO currentWeaponSO;
    private Coroutine delayedWeaponSpawn;

    void Start()
    {
        SpawnRandomWeapon();
    }

    private void Update()
    {
        CheckCollisions();
    }

    void CheckCollisions()
    {
        if (delayedWeaponSpawn != null)
        {
            return;
        }
        
        Collider2D[] colliders = Physics2D.OverlapBoxAll(pickupCollider.bounds.center, pickupCollider.bounds.size, 0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out PlayerWeapon playerWeapon))
            {
                weaponSprite.enabled = false;
                playerWeapon.PickUpWeapon(currentWeaponSO);

                if (delayedWeaponSpawn == null)
                {
                    delayedWeaponSpawn = StartCoroutine(SpawnWeaponAfterSeconds(spawnDelaySeconds));
                    break;
                }
            }
        }
    }

    private IEnumerator SpawnWeaponAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SpawnRandomWeapon();
        delayedWeaponSpawn = null;
    }

    private void SpawnRandomWeapon()
    {
        currentWeaponSO = weaponSOs[Random.Range(0, weaponSOs.Length)];
        weaponSprite.sprite = currentWeaponSO.sprite;
        weaponSprite.enabled = true;
    }
}