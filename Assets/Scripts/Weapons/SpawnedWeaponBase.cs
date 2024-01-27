using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SpawnedWeaponBase : MonoBehaviour
{
    public PlayerInput OwnerPlayer;

    // Use this for one frame only checks
    [SerializeField] private float _overlapQueryRadius = 0f;
    [SerializeField] private float _lifeTime = 3f;

    private List<Collider2D> contacts = new List<Collider2D>();

    private void Awake()
    {
        if (_overlapQueryRadius > float.Epsilon
            && Physics2D.OverlapCircle(transform.position, _overlapQueryRadius, new ContactFilter2D(), contacts) > 0) { }

        {
            foreach (Collider2D coll in contacts)
            {
                if (coll.CompareTag("Player")) { return; }

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

        OnAwake();
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

    protected virtual void OnOverlapSelf() { }

    protected virtual void OnOverlapEnemy(Collider2D other) { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { return; }

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
        if (other.CompareTag("Player")) { return; }

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
        if (other.CompareTag("Player")) { return; }

        if (other.gameObject == OwnerPlayer.gameObject)
        {
            OnExitSelf();
        }
        else
        {
            OnExitEnemy(other);
        }
    }

    protected virtual void OnHitSelf() { }

    protected virtual void OnStaySelf() { }

    protected virtual void OnExitSelf() { }

    protected virtual void OnHitEnemy(Collider2D other) { }

    protected virtual void OnStayEnemy(Collider2D other) { }

    protected virtual void OnExitEnemy(Collider2D other) { }
}
