using System;
using System.Collections.Generic;
using Hassus.Map;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 10;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _groundCheckRadius = 0.5f;
    [SerializeField] private List<AudioClip> walkSounds;

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
    private float _jumpCooldown = 0.0f;
    private const float _jumpBufferLength = 0.1f;
    private const float _jumpCooldownLength = 0.2f;

    private AudioSource audioSource;

    private bool canMove = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = MapLoader.Instance.GetRandomSpawnPoint();

        GameStateSystem.OnGameStateChanged += OnGameStateChanged;
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        GameStateSystem.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState state)
    {
        canMove = state == GameState.FIGHT;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position + (Vector3)_groundCheckOffset, _groundCheckRadius);
    }
#endif

    private void Update()
    {
        if (!canMove) { return; }

        Grounded = Physics2D.OverlapCircle((Vector2)transform.position + _groundCheckOffset, _groundCheckRadius, _groundCheckLayer);

        if (_jumpBuffer > 0f && Grounded)
        {
            _jumpBuffer = 0.0f;
            Jump();
        }

        _jumpCooldown -= Time.deltaTime;
        _jumpBuffer -= Time.deltaTime;

        if (_input == Vector2.zero)
        {
            return;
        }

        LastDirection = Mathf.RoundToInt(Mathf.Sign(_input.x));
        rb.velocity = new Vector2(_input.x * _speed, rb.velocity.y);

        if (!audioSource.isPlaying && Grounded)
        {
            audioSource.PlayOneShot(walkSounds[UnityEngine.Random.Range(0, walkSounds.Count)]);
        }
    }

    public void Jump()
    {
        if (_jumpCooldown > 0.0f) { return; }

        SFXManager.Instance.PlayOneShot(SFXType.BoingRandom);
        
        _jumpCooldown = _jumpCooldownLength;
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
        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity += Vector2.up * 6;
        rb.AddTorque(LastDirection * 20);
        enabled = false;
    }

    public void PlayerRespawn()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.rotation = Quaternion.identity;
        enabled = true;
    }
}
