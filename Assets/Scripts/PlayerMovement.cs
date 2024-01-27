using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundCheckRadius = 0.5f;

    private Rigidbody2D rb;


    [SerializeField] private Vector2 _groundCheckOffset;
    [SerializeField] private LayerMask _groundCheckLayer;

    public Vector2 Input => _input;
    public bool Grounded { get; private set; } = true;
    public int LastDirection { get; private set; }
    public int JumpDirection
    {
        get
        {
            return Mathf.RoundToInt(rb.velocity.y);
        }
    }
    private Vector2 _input;
    private float _jumpBuffer = 0.0f;
    private const float _jumpBufferLength = 0.1f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + (Vector3)_groundCheckOffset, _groundCheckRadius);
    }
#endif

    private void Update()
    {
        Grounded = Physics2D.OverlapCircle((Vector2)transform.position + _groundCheckOffset, _groundCheckRadius, _groundCheckLayer);

        if (_jumpBuffer > 0f && Grounded)
        {
            _jumpBuffer = 0.0f;
            Jump();
        }

        _jumpBuffer -= Time.deltaTime;

        if (_input == Vector2.zero)
        {
            return;
        }

        LastDirection = Mathf.RoundToInt(Mathf.Sign(_input.x));
        rb.velocity = new Vector2(_input.x * _speed, rb.velocity.y);
    }

    public void Jump()
    {
        rb.velocity = Vector2.up * _jumpForce;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();

        if (_input.y > float.Epsilon)
        {
            _jumpBuffer = _jumpBufferLength;
        }

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
