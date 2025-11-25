using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum CardState { FaceDown, FaceUp, Matched }

public class Card : MonoBehaviour
{
    public int id;
    public Sprite frontSprite;
    public Sprite backSprite;
    public Sprite emptySprite; // optional placeholder sprite

    [HideInInspector] public CardState State = CardState.FaceDown;

    private Image img;
    private Button btn;

    private void Awake()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();

        if (img != null) img.sprite = backSprite;
        if (btn != null) btn.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (State != CardState.FaceDown) return;
        GameManager.Instance.OnCardSelected(this);
    }

    public void Flip(bool faceUp, Action onComplete = null)
    {
        if (State == CardState.Matched) return;
        StopAllCoroutines();
        StartCoroutine(FlipRoutine(faceUp, onComplete));
    }

    private IEnumerator FlipRoutine(bool faceUp, Action onComplete)
    {
        float duration = 0.25f;
        float t = 0f;

        // First half flip
        while (t < duration)
        {
            t += Time.deltaTime;
            float scale = Mathf.Lerp(1f, 0f, t / duration);
            transform.localScale = new Vector3(scale, 1f, 1f);
            yield return null;
        }

        // Swap sprite
        State = faceUp ? CardState.FaceUp : CardState.FaceDown;
        img.sprite = faceUp ? frontSprite : backSprite;

        // Optional: play flip sound
        AudioManager.Instance?.PlayFlip();

        // Second half flip
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
        State = CardState.Matched;

        // Keep object active but visually empty
        img.sprite = emptySprite != null ? emptySprite : null;
        img.color = new Color(1, 1, 1, 0); // optional fully transparent
        btn.interactable = false;
    }
}
