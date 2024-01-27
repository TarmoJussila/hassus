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

    public void UseWeapon(InputAction.CallbackContext context)
    {
        if (context.performed) { return; }

        if (usesLeft <= 0)
        {
            // TODO: Taunt?
        }

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

        weapon.transform.position = transform.position + (Vector3)currentWeapon.SpawnOffset;

        if (currentWeapon.SpawnForce != Vector2.zero)
        {
            weapon.GetComponent<Rigidbody2D>()
                .AddForce(new Vector2(_movement.LastDirection * currentWeapon.SpawnForce.x, currentWeapon.SpawnForce.y));
        }

        Debug.Log("Player: " + weapon.OwnerPlayer.playerIndex + " used " + currentWeapon.name);

        if (usesLeft <= 0)
        {
            Disarm();
        }
    }

    public void UseWeapon() { }

    public void Disarm()
    {
        currentWeapon = null;
        spriteRenderer.enabled = false;
    }

    private void Update()
    {
        spriteRenderer.flipX = _movement.LastDirection < 0;
        Vector3 pos = spriteRenderer.transform.localPosition;
        pos.x = _movement.LastDirection * 0.5f;
        spriteRenderer.transform.localPosition = pos;
    }
}
