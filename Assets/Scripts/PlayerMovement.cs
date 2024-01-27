using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _jumpForce = 10f;
    
    private Rigidbody2D rb;

    private Vector2 _input;

    [SerializeField] private Vector2 _groundCheckOffset;
    [SerializeField] private LayerMask _groundCheckLayer;

    private bool _grounded = true;
    private bool _jumped = false;

    public int LastDirection { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_input == Vector2.zero)
        {
            return;
        }

        _grounded = Physics2D.OverlapCircle((Vector2)transform.position + _groundCheckOffset, .3f, _groundCheckLayer);

        LastDirection = Mathf.RoundToInt(Mathf.Sign(_input.x));
        rb.velocity = new Vector2(_input.x * _speed, 0);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && !_jumped)
        {
            rb.velocity = Vector2.up * _jumpForce;
            _jumped = true;
        }
        else
        {
            _jumped = false;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _input.y = 0;
        _input.Normalize();
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Fire!");
        }
    }

    public void PlayerDead()
    {
        throw new System.NotImplementedException();
    }

    public void PlayerRespawn()
    {
        throw new System.NotImplementedException();
    }
}
