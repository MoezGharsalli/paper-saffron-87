using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;      // assign in inspector
    public TMP_Text bestScoreText;  // assign in inspector
    public TMP_Text comboText;

    private int bestScore = 0;

    void Start()
    {
        LoadBestScore();
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        // Score UI
        if (scoreText != null)
            scoreText.SetText("Score: " + GameManager.Instance.score);

        // Best score UI + save
        if (GameManager.Instance.score > bestScore)
        {
            bestScore = GameManager.Instance.score;
            SaveBestScore();
        }

        if (bestScoreText != null)
            bestScoreText.SetText("Best: " + bestScore);

        // Combo UI
        if (comboText != null)
        {
            int combo = GameManager.Instance.comboCount;

            if (combo > 1)
                comboText.SetText("Combo x" + combo);
            else
                comboText.SetText(""); // hide combo for first match
        }
    }

    public IEnumerator PunchCombo()
    {
        if (comboText == null) yield break;

        Vector3 originalScale = comboText.transform.localScale;
        Vector3 targetScale = originalScale * 1.5f; // bigger pop

        float duration = 0.3f; // longer, smoother
        float t = 0f;

        // Scale up
        while (t < duration)
        {
            t += Time.deltaTime;
            comboText.transform.localScale = Vector3.Lerp(originalScale, targetScale, Mathf.SmoothStep(0f, 1f, t / duration));
            yield return null;
        }

        // Scale back down
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            comboText.transform.localScale = Vector3.Lerp(targetScale, originalScale, Mathf.SmoothStep(0f, 1f, t / duration));
            yield return null;
        }

        comboText.transform.localScale = originalScale;
    }

    private void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save();
    }

    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    public void ResetScore()
    {
        GameManager.Instance?.ResetScore();
    }
}
