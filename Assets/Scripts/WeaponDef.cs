using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponSO", order = 1)]
public class WeaponDef : ScriptableObject
{
    [FormerlySerializedAs("sprite")] public Sprite Sprite;
    public int MaxUses;
    [FormerlySerializedAs("PlayerAnimation")] public string CharacterAnimation;
    public string WeaponAnimation;
    public GameObject Prefab;
}