using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private WeaponDef currentWeapon;
    private int usesLeft;

    private PlayerAnimator animator;
    private PlayerMovement _movement;

    private void Awake()
    {
        animator = GetComponent<PlayerAnimator>();
        _movement = GetComponent<PlayerMovement>();
    }

    public void PickUpWeapon(WeaponDef weaponDef)
    {
        currentWeapon = weaponDef;
        usesLeft = weaponDef.MaxUses;

        spriteRenderer.sprite = weaponDef.Sprite;
        spriteRenderer.enabled = true;
    }

    public void UseWeapon()
    {
        usesLeft--;

        if (string.IsNullOrEmpty(currentWeapon.CharacterAnimation))
        {
            animator.CharacterAnimation(currentWeapon.CharacterAnimation);
        }

        if (string.IsNullOrEmpty(currentWeapon.WeaponAnimation))
        {
            animator.WeaponAnimation(currentWeapon.WeaponAnimation);
        }

        SpawnedWeaponBase weapon = Instantiate(currentWeapon.Prefab);
        weapon.transform.position = transform.position;
        weapon.OwnerPlayer = gameObject;
    }

    private void Update()
    {
        spriteRenderer.flipX = _movement.LastDirection < 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UseWeapon();
        }
    }
}
