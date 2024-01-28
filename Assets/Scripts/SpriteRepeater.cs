using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class SpriteRepeater : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float speed = 10f;

    private float timer;
    private int spriteIndex = 0;
    
    private void Update()
    {
        timer += Time.deltaTime * speed;
        if (timer > 1f)
        {
            timer = 0f;
            spriteIndex++;
            if (spriteIndex >= sprites.Length)
            {
                spriteIndex = 0;
            }
            image.sprite = sprites[spriteIndex];
        }
    }
}
