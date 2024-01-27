using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    public bool HasWeapon => currentWeapon != null;
    
    [SerializeField] private SpriteRenderer spriteRenderer;

    private WeaponDef currentWeapon;
    private int usesLeft;

    private PlayerAnimator _animator;
    private PlayerMovement _movement;
    private PlayerInput _input;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _animator = GetComponent<PlayerAnimator>();
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
            _animator.CharacterAnimation(currentWeapon.CharacterAnimation);
        }

        if (string.IsNullOrEmpty(currentWeapon.WeaponAnimation))
        {
            _animator.WeaponAnimation(currentWeapon.WeaponAnimation);
        }

        SpawnedWeaponBase weapon = Instantiate(currentWeapon.Prefab);
        weapon.OwnerPlayer = _input;

        if (usesLeft <= 0)
        {
            Disarm();
        }
    }

    public void Disarm()
    {
        currentWeapon = null;
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        spriteRenderer.flipX = _movement.LastDirection < 0;
    }
}
