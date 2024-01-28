using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeapon : MonoBehaviour
{
    public bool HasWeapon => currentWeapon != null;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<AudioClip> tauntSounds;

    private WeaponDef currentWeapon;
    private int usesLeft;

    private PlayerAnimator _animator;
    private PlayerMovement _movement;
    private PlayerInput _input;

    private float _cooldown = 0.0f;

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
        if (!context.started || _cooldown > 0.0f || GameStateSystem.Instance.CurrentState != GameState.FIGHT) { return; }

        if (currentWeapon == null)
        {
            PlayTauntSound();
            return;
        }

        if (usesLeft < 1) { return; }

        _cooldown = currentWeapon.Cooldown;

        usesLeft--;

        if (!string.IsNullOrEmpty(currentWeapon.WeaponAnimationTrigger))
        {
            _animator.WeaponAnimation(currentWeapon.WeaponAnimationTrigger);
        }

        SpawnedWeaponBase weapon = Instantiate(
            currentWeapon.Prefab,
            transform.position + new Vector3(currentWeapon.SpawnOffset.x * _movement.LastDirection, currentWeapon.SpawnOffset.y),
            Quaternion.identity
        );
        weapon.OwnerPlayer = _input;

        if (currentWeapon.SpawnForce != Vector2.zero)
        {
            Rigidbody2D rb = weapon.GetComponent<Rigidbody2D>();
            rb.AddForce(new Vector2(_movement.LastDirection * currentWeapon.SpawnForce.x, currentWeapon.SpawnForce.y));
            rb.AddTorque(_movement.LastDirection * currentWeapon.spawnRotationForce);
        }

        Debug.Log("Player: " + weapon.OwnerPlayer.playerIndex + " used " + currentWeapon.name);

        if (usesLeft <= 0)
        {
            Disarm();
        }
    }

    private void PlayTauntSound()
    {
        GetComponent<AudioSource>().PlayOneShot(tauntSounds[UnityEngine.Random.Range(0, tauntSounds.Count)]);
    }

    public void UseWeapon() { }

    public void Disarm()
    {
        if (currentWeapon == null) { return; }

        StartCoroutine(DelaySpriteHide());
    }

    private IEnumerator DelaySpriteHide()
    {
        yield return new WaitForSeconds(currentWeapon.spriteHideDelay);
        spriteRenderer.enabled = false;
        currentWeapon = null;
    }

    private void Update()
    {
        _cooldown -= Time.deltaTime;

        spriteRenderer.flipX = _movement.LastDirection < 0;
        Vector3 pos = spriteRenderer.transform.localPosition;
        pos.x = _movement.LastDirection * 0.3f;
        spriteRenderer.transform.localPosition = pos;
    }
}
