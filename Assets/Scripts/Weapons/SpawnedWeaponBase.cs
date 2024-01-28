using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SpawnedWeaponBase : MonoBehaviour
{
    [HideInInspector] public PlayerInput OwnerPlayer;

    // Use this for one frame only checks
    [SerializeField] private float _overlapQueryRadius = 0f;
    [SerializeField] private float _lifeTime = 3f;

    private List<Collider2D> contacts = new List<Collider2D>();

    private void Start()
    {
        if (_overlapQueryRadius > float.Epsilon)
        {
            
            foreach (Collider2D coll in Physics2D.OverlapCircleAll(transform.position, _overlapQueryRadius))
            {
                if (!coll.CompareTag("Player")) { break; }

                if (coll.gameObject == OwnerPlayer.gameObject)
                {
                    OnOverlapSelf();
                }
                else
                {
                    OnOverlapEnemy(coll);
                }
            }
        }

        if (_lifeTime > 0f)
        {
            StartCoroutine(DestroyTimer());
        }

        OnStart();
    }

    private IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(_lifeTime);
        DestroyWeapon();
    }

    protected void DestroyWeapon()
    {
        Destroy(gameObject);
    }

    protected virtual void OnAwake() { }

    protected virtual void OnStart() { }

    protected virtual void OnOverlapSelf() { }

    protected virtual void OnOverlapEnemy(Collider2D other) { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) { return; }

        if (other.gameObject == OwnerPlayer.gameObject)
        {
            OnHitSelf();
        }
        else
        {
            OnHitEnemy(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) { return; }

        if (other.gameObject == OwnerPlayer.gameObject)
        {
            OnStaySelf();
        }
        else
        {
            OnStayEnemy(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) { return; }

        if (other.gameObject == OwnerPlayer.gameObject)
        {
            OnExitSelf();
        }
        else
        {
            OnExitEnemy(other);
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (!coll.collider.CompareTag("Player"))
        {
            OnCollideOther();
        }
        else if (coll.gameObject == OwnerPlayer.gameObject)
        {
            OnCollideSelf();
        }
        else
        {
            OnCollideEnemy(coll);
        }
    }

    protected virtual void OnCollideOther() { }

    protected virtual void OnCollideSelf() { }

    protected virtual void OnCollideEnemy(Collision2D coll) { }

    protected virtual void OnHitSelf() { }

    protected virtual void OnStaySelf() { }

    protected virtual void OnExitSelf() { }

    protected virtual void OnHitEnemy(Collider2D other) { }

    protected virtual void OnStayEnemy(Collider2D other) { }

    protected virtual void OnExitEnemy(Collider2D other) { }
}
