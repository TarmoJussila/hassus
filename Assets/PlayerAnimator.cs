using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private List<AnimatorOverrideController> _overrides;
    [SerializeField] private SpriteRenderer _sprite;

    [SerializeField] private Animator _charAnimator;
    [SerializeField] private Animator _weaponAnimator;

    private PlayerMovement _movement;
    private PlayerInput _input;

    private static readonly int dead = Animator.StringToHash("Dead");
    private static readonly int grounded = Animator.StringToHash("Grounded");
    private static readonly int jumpDir = Animator.StringToHash("JumpDir");
    private static readonly int movement = Animator.StringToHash("Movement");
    private static readonly int direction = Animator.StringToHash("direction");

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
        _charAnimator.runtimeAnimatorController = _overrides[_input.playerIndex];
    }

    private void Update()
    {
        _sprite.flipX = _movement.LastDirection < 0;

        _charAnimator.SetBool(grounded, _movement.Grounded);
        _charAnimator.SetInteger(jumpDir, _movement.JumpDirection);
        _charAnimator.SetInteger(movement, Mathf.RoundToInt(_movement.Input.x));
        _weaponAnimator.SetInteger(direction, _movement.LastDirection);
    }

    public void WeaponAnimation(string trigger)
    {
        _weaponAnimator.SetTrigger(trigger);
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
