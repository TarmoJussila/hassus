using System.Collections;
using System.Collections.Generic;
using Hassus.Map;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int Radius;
    public int SpawnerPlayer = -1;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Collider2D coll in Physics2D.OverlapCircleAll(transform.position, Radius))
        {
            if (coll.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                health.TakeDamage(500, SpawnerPlayer);
            }
        }

        SFXManager.Instance.PlayOneShot(SFXType.ExplosionRandom);
        MapLoader.Instance.Explode(transform.position, Radius);
    }
}
