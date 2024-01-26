using System.Collections;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField] private GameObject entityToSpawn;
    [SerializeField] private WeaponSO[] weaponSOs;
    [SerializeField] private float spawnDelaySeconds = 1;
    [SerializeField] private SpriteRenderer weaponSprite;

    private BoxCollider2D boxCollider;
    private WeaponSO currentWeaponSO;

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
            if (collider.TryGetComponent(out PlayerWeapon playerWeapon))
            {
                weaponSprite.enabled = false;
                playerWeapon.PickUpWeapon(currentWeaponSO);
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
        WeaponSO randomWeaponSO = weaponSOs[Random.Range(0, weaponSOs.Length)];
        weaponSprite.sprite = randomWeaponSO.sprite;
        weaponSprite.enabled = true;
    }
}