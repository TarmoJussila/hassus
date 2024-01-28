using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteFader : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private float delay = 0f;
    
    private readonly float fadeInSpeed = 0.5f;
    private readonly float fadeOutSpeed = 0.5f;

    private void OnEnable()
    {
        if (fadeIn)
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 0f);
            }
            FadeIn(delay);
        }
        else
        {
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, 1f);
            }
            FadeOut(delay);
        }
    }

    public void FadeIn(float delay)
    {
        StartCoroutine(FadeInCoroutine(delay));
    }
    
    public void FadeOut(float delay)
    {
        StartCoroutine(FadeOutCoroutine(delay));
    }
    
    private IEnumerator FadeInCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeInSpeed;
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, Mathf.Lerp(0f, 1f, timer));
            }
            yield return null;
        }
        yield break;
    }
    
    private IEnumerator FadeOutCoroutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        
        float timer = 0f;
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeInSpeed;
            for (int i = 0; i < images.Length; i++)
            {
                images[i].color = new Color(images[i].color.r, images[i].color.g, images[i].color.b, Mathf.Lerp(1f, 0f, timer));
            }
            yield return null;
        }
        yield break;
    }
}
