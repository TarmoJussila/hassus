using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void PickUpWeapon(WeaponSO weaponSO)
    {
        spriteRenderer.sprite =
            weaponSO.sprite;
        spriteRenderer.enabled = true;
    }

    private void Update()
    {
        // TODO: mirror weapon when facing other direction
    }
}
