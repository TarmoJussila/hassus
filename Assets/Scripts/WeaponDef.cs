using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponDef", order = 1)]
public class WeaponDef : ScriptableObject
{
    [FormerlySerializedAs("sprite")] public Sprite Sprite;
    public int MaxUses;
    [FormerlySerializedAs("PlayerAnimation")] public string CharacterAnimation;
    public string WeaponAnimation;
    public SpawnedWeaponBase Prefab;
    public Vector2 SpawnOffset;
    public Vector2 SpawnForce;
}