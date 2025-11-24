using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int cardID; // unique identifier for matching
    public bool isFlipped = false;
    public Sprite frontSprite;
    public Sprite backSprite;
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = backSprite; // start showing back
    }

    public void Flip()
    {
        isFlipped = !isFlipped;
        sr.sprite = isFlipped ? frontSprite : backSprite;
    }
}
