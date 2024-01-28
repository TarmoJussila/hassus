using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundWithDelay : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float delay = 0f;
    
    private void Start()
    {
        audioSource.PlayDelayed(delay);
    }
}
