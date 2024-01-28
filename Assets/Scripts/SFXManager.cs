using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SFXType
{
    Bonk,
    Bruh,
    ExplosionRandom,
    FartRandom,
    Wobble,
    SqueakyRandom,
    Vineboom,
    ChickenRandom,
    BoingRandom,
    Pop
}

[System.Serializable]
public class SFXPair
{
    public SFXType Type;
    public float Volume = 1;
    public AudioClip[] Clips;
}

public class SFXManager : MonoSingleton<SFXManager>
{
    [SerializeField] private List<SFXPair> _sfxPairs = new();

    [SerializeField] private AudioSource _source;

    public void PlayOneShot(SFXType type)
    {
        var pair = _sfxPairs.FirstOrDefault(pair => pair.Type == type);
        AudioClip[] clips = pair.Clips;
        _source.PlayOneShot(clips[Random.Range(0, clips.Length)], pair.Volume);
    }
}
