using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedWeaponBase : MonoBehaviour
{
    public GameObject OwnerPlayer;
    
    // Use this and don't put a collider if you want a one frame check
    public float OverlapQueryRadius = 0f;

    private List<Collider2D> contacts = new List<Collider2D>();

    private void Awake()
    {
        if (OverlapQueryRadius > float.Epsilon
            && Physics2D.OverlapCircle(transform.position, OverlapQueryRadius, new ContactFilter2D(), contacts) > 0) { }

        {
            foreach (Collider2D coll in contacts)
            {
                OnTriggerEnter2D(coll);
            }
        }

        OnAwake();
    }

    protected virtual void OnAwake() { }

    protected virtual void OnOverlapSelf() { }

    protected virtual void OnOverlapEnemy() { }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) { return; }

        if (other.gameObject == OwnerPlayer)
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

        if (other.gameObject == OwnerPlayer)
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

        if (other.gameObject == OwnerPlayer)
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
