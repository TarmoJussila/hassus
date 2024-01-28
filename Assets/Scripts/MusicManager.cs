using System.Collections;
using UnityEngine;

public class MusicManager : MonoSingleton<MusicManager>
{
    public enum MusicType
    {
        INTRO = 1,
        GAMEPLAY = 2,
        OUTRO = 3
    }
    
    [SerializeField] private AudioSource introMusicSource;
    [SerializeField] private AudioSource gameplayMusicSource;
    [SerializeField] private AudioSource outroMusicSource;

    private readonly float fadeInSpeed = 0.2f;
    private readonly float fadeOutSpeed = 0.5f;
    private readonly float volumeMax = 0.5f;

    protected override void OnAwake()
    {
        base.OnAwake();
        introMusicSource.volume = 0f;
        gameplayMusicSource.volume = 0f;
        outroMusicSource.volume = 0f;
    }

    public void FadeInMusic(MusicType type)
    {
        StartCoroutine(FadeInMusicCoroutine(type));
    }
    
    public void FadeOutMusic(MusicType type)
    {
        StartCoroutine(FadeOutMusicCoroutine(type));
    }
    
    private IEnumerator FadeInMusicCoroutine(MusicType type)
    {
        float initialVolume = 0f;
        if (type == MusicType.INTRO)
        {
            initialVolume = introMusicSource.volume;
            introMusicSource.Play();
        }
        else if (type == MusicType.GAMEPLAY)
        {
            initialVolume = gameplayMusicSource.volume;
            gameplayMusicSource.Play();
        }
        else if (type == MusicType.OUTRO)
        {
            initialVolume = outroMusicSource.volume;
            outroMusicSource.Play();
        }
        
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeInSpeed;
            if (type == MusicType.INTRO)
            {
                introMusicSource.volume = Mathf.Lerp(initialVolume, volumeMax, timer);
            }
            else if (type == MusicType.GAMEPLAY)
            {
                gameplayMusicSource.volume = Mathf.Lerp(initialVolume, volumeMax, timer);
            }
            else if (type == MusicType.OUTRO)
            {
                outroMusicSource.volume = Mathf.Lerp(initialVolume, volumeMax, timer);
            }
            yield return null;
        }
        yield break;
    }
    
    private IEnumerator FadeOutMusicCoroutine(MusicType type)
    {
        float initialVolume = 0f;
        if (type == MusicType.INTRO)
        {
            initialVolume = introMusicSource.volume;
        }
        else if (type == MusicType.GAMEPLAY)
        {
            initialVolume = gameplayMusicSource.volume;
        }
        else if (type == MusicType.OUTRO)
        {
            initialVolume = outroMusicSource.volume;
        }
        
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeOutSpeed;
            if (type == MusicType.INTRO)
            {
                introMusicSource.volume = Mathf.Lerp(initialVolume, 0f, timer);
            }
            else if (type == MusicType.GAMEPLAY)
            {
                gameplayMusicSource.volume = Mathf.Lerp(initialVolume, 0f, timer);
            }
            else if (type == MusicType.OUTRO)
            {
                outroMusicSource.volume = Mathf.Lerp(initialVolume, 0f, timer);
            }
            yield return null;
        }
        
        if (type == MusicType.INTRO)
        {
            introMusicSource.Stop();
        }
        else if (type == MusicType.GAMEPLAY)
        {
            gameplayMusicSource.Stop();
        }
        else if (type == MusicType.OUTRO)
        {
            outroMusicSource.Stop();
        }
        yield break;
    }
}
