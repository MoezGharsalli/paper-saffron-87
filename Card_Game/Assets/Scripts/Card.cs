using System;
using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    public int id;
    public Sprite frontSprite;
    public Sprite backSprite;

    [HideInInspector] public bool IsFaceUp = false;
    [HideInInspector] public bool IsMatched = false;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = backSprite;
    }

    void OnMouseDown()
    {
        if (IsMatched || IsFaceUp) return;
        GameManager.Instance.OnCardSelected(this);
    }

    public void Flip(bool faceUp, Action onComplete = null)
    {
        if (IsMatched) return;
        StopAllCoroutines();
        StartCoroutine(FlipRoutine(faceUp, onComplete));
    }

    private IEnumerator FlipRoutine(bool faceUp, Action onComplete)
    {
        float duration = 0.25f;
        float t = 0f;

        // First half: scale X from 1 → 0
        while (t < duration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, t / duration);
            transform.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        // Switch sprite
        IsFaceUp = faceUp;
        sr.sprite = faceUp ? frontSprite : backSprite;

        // Optional: play flip sound if AudioManager exists
        if (AudioManager.Instance != null) AudioManager.Instance.PlayFlip();

        // Second half: scale X from 0 → 1
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(0f, 1f, t / duration);
            transform.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        transform.localScale = Vector3.one;
        onComplete?.Invoke();
    }

    public void MarkMatched()
    {
        IsMatched = true;
        IsFaceUp = true;
    }
}
