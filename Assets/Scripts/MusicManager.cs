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

    private readonly float fadeInSpeed = 0.1f;
    private readonly float fadeOutSpeed = 0.5f;

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
        if (type == MusicType.INTRO)
        {
            introMusicSource.Play();
        }
        else if (type == MusicType.GAMEPLAY)
        {
            gameplayMusicSource.Play();
        }
        else if (type == MusicType.OUTRO)
        {
            outroMusicSource.Play();
        }
        
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeInSpeed;
            if (type == MusicType.INTRO)
            {
                introMusicSource.volume = Mathf.Lerp(introMusicSource.volume, 1f, timer);
            }
            else if (type == MusicType.GAMEPLAY)
            {
                gameplayMusicSource.volume = Mathf.Lerp(gameplayMusicSource.volume, 1f, timer);
            }
            else if (type == MusicType.OUTRO)
            {
                outroMusicSource.volume = Mathf.Lerp(outroMusicSource.volume, 1f, timer);
            }
            yield return null;
        }
        yield break;
    }
    
    private IEnumerator FadeOutMusicCoroutine(MusicType type)
    {
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeOutSpeed;
            if (type == MusicType.INTRO)
            {
                introMusicSource.volume = Mathf.Lerp(introMusicSource.volume, 0f, timer);
            }
            else if (type == MusicType.GAMEPLAY)
            {
                gameplayMusicSource.volume = Mathf.Lerp(gameplayMusicSource.volume, 0f, timer);
            }
            else if (type == MusicType.OUTRO)
            {
                outroMusicSource.volume = Mathf.Lerp(outroMusicSource.volume, 0f, timer);
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
