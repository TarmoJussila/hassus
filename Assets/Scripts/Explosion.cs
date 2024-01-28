using System.Collections;
using System.Collections.Generic;
using Hassus.Map;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int Radius;
    public int SpawnerPlayer = -1;
    public int Damage = 500; 

    // Start is called before the first frame update
    void Start()
    {
        foreach (Collider2D coll in Physics2D.OverlapCircleAll(transform.position, Radius))
        {
            if (coll.TryGetComponent<PlayerHealth>(out PlayerHealth health))
            {
                health.TakeDamage(Damage, SpawnerPlayer);
            }
        }

        SFXManager.Instance.PlayOneShot(SFXType.ExplosionRandom);
        MapLoader.Instance.Explode(transform.position, Radius);
    }
}
