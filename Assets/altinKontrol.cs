using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class altinKontrol : MonoBehaviour
{
    public Sprite[] animasyonKareleri;
    SpriteRenderer spriteRenderer;
    float zaman = 0;
    int animasyonKareleriSayaci = 0;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        zaman += Time.deltaTime;
        if (zaman > 0.05f)
        {
            spriteRenderer.sprite = animasyonKareleri[animasyonKareleriSayaci+1];
            if (animasyonKareleri.Length == animasyonKareleriSayaci)
            {
                animasyonKareleriSayaci = 0;
            }
            zaman = 0;
        }
    }
}
