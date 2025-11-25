using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TMP_Text scoreText;      // assign in inspector
    public TMP_Text bestScoreText;  // assign in inspector

    private int bestScore = 0;

    void Start()
    {
        LoadBestScore();
    }

    void Update()
    {
        if (GameManager.Instance == null) return;

        scoreText?.SetText("Score: " + GameManager.Instance.score);

        if (GameManager.Instance.score > bestScore)
        {
            bestScore = GameManager.Instance.score;
            SaveBestScore();
        }

        bestScoreText?.SetText("Best: " + bestScore);
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
        if (GameManager.Instance != null)
            GameManager.Instance.score = 0;
    }
}
