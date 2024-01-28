using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponDef", order = 1)]
public class WeaponDef : ScriptableObject
{
    [FormerlySerializedAs("sprite")] public Sprite Sprite;
    public int MaxUses;
    public string WeaponAnimationTrigger;
    public SpawnedWeaponBase Prefab;
    public Vector2 SpawnOffset;
    public Vector2 SpawnForce;
    public float Cooldown = 0.1f;
    public float spawnRotationForce;
    public float spriteHideDelay;
}