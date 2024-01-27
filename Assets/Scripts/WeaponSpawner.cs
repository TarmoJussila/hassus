using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private WeaponDef[] weaponDefs;
    [SerializeField] private float spawnDelaySeconds = 1;
    [SerializeField] private SpriteRenderer weaponSprite;
    [SerializeField] private BoxCollider2D pickupCollider;

    private WeaponDef currentWeaponDef;
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
            if (collider.TryGetComponent(out PlayerWeapon playerWeapon) && !playerWeapon.HasWeapon)
            {
                weaponSprite.enabled = false;
                playerWeapon.PickUpWeapon(currentWeaponDef);

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
        currentWeaponDef = weaponDefs.Length > 0 ? weaponDefs[Random.Range(0, weaponDefs.Length)] : WeaponManager.Instance.GetRandomWeapon();
        weaponSprite.sprite = currentWeaponDef.Sprite;
        weaponSprite.enabled = true;
    }
}
