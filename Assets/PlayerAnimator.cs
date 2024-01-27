using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;

    private PlayerMovement _movement;
    private Animator _charAnimator;
    private Animator _weaponAnimator;
    private static readonly int dead = Animator.StringToHash("Dead");
    private static readonly int grounded = Animator.StringToHash("Grounded");
    private static readonly int jumpDir = Animator.StringToHash("JumpDir");
    private static readonly int movement = Animator.StringToHash("Movement");

    private void Awake()
    {
        _movement = GetComponent<PlayerMovement>();
        _charAnimator = GetComponentInChildren<Animator>();
        _weaponAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        _sprite.flipX = _movement.LastDirection < 0;

        _charAnimator.SetBool(grounded, _movement.Grounded);
        _charAnimator.SetInteger(jumpDir, _movement.JumpDirection);
        _charAnimator.SetInteger(movement, Mathf.RoundToInt(_movement.Input.x));
    }

    public void CharacterAnimation(string animation)
    {
        _charAnimator.Play(animation);
    }

    public void WeaponAnimation(string animation)
    {
        _weaponAnimator.Play(animation);
    }

    public void PlayerDead()
    {
        _charAnimator.SetBool(dead, true);
    }

    public void PlayerRespawn()
    {
        _charAnimator.SetBool(dead, false);
    }
}
