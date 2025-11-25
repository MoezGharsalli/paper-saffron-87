using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public BoardManager board;
    public ScoreManager scoreManager;
    public GameObject victoryPanel;

    public int comboBonusPerChain = 5;
    public int comboCount = 0;
    public int maxCombo = 0;
    public int bonusScore = 0;

    public Button grid2x2Button;
    public Button grid3x4Button;
    public Button grid4x4Button;

    private List<Card> selected = new List<Card>();
    private bool comparing;

    public int score { get; private set; } = 0;
    [HideInInspector] public int rows = 2;
    [HideInInspector] public int columns = 2;

    private void Awake() { Instance = this; }

    private void Start() { GenerateBoard(rows, columns); }

    public void GenerateBoard(int r, int c)
    {
        rows = r;
        columns = c;
        score = 0;
        selected.Clear();
        board.GenerateBoard(rows, columns);
    }

    public void OnCardSelected(Card card)
    {
        if (comparing || card.State != CardState.FaceDown) return;

        card.Flip(true, () =>
        {
            selected.Add(card);
            if (selected.Count >= 2) StartCoroutine(CompareRoutine());
        });
    }

    IEnumerator CompareRoutine()
    {
        comparing = true;
        yield return new WaitForSeconds(0.25f);

        Card a = selected[0];
        Card b = selected[1];

        if (a.id == b.id)
        {
            a.MarkMatched();
            b.MarkMatched();

            // Increase combo
            comboCount++;
            maxCombo = Mathf.Max(maxCombo, comboCount);
            StartCoroutine(scoreManager.PunchCombo());

            // Base score
            int points = 10;

            // Give bonus when combo continues
            if (comboCount > 1)
            {
                int bonus = (comboCount - 1) * comboBonusPerChain;
                points += bonus;
                bonusScore += bonus;
                Debug.Log($"COMBO x{comboCount} | Bonus +{bonus} | Total Bonus: {bonusScore}");
            }

            score += points;
        }
        else
        {
            // Wrong pair → reset combo
            comboCount = 0;

            score -= 2;
            yield return new WaitForSeconds(0.4f);
            a.Flip(false);
            b.Flip(false);
        }

        selected.Remove(a);
        selected.Remove(b);
        comparing = false;

        // Victory check
        bool allMatched = true;
        foreach (var card in board.cards)
        {
            if (card.State != CardState.Matched)
            {
                allMatched = false;
                break;
            }
        }

        if (allMatched)
        {
            Victory();
        }
    }

    void Victory()
    {
        // Show victory panel
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        // Disable grid buttons
        if (grid2x2Button != null) grid2x2Button.interactable = false;
        if (grid3x4Button != null) grid3x4Button.interactable = false;
        if (grid4x4Button != null) grid4x4Button.interactable = false;

        Debug.Log("Victory!");
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void RestartGame()
    {
        // Reset combo system
        comboCount = 0;
        bonusScore = 0;

        // Hide victory panel
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        // Enable grid buttons
        if (grid2x2Button != null) grid2x2Button.interactable = true;
        if (grid3x4Button != null) grid3x4Button.interactable = true;
        if (grid4x4Button != null) grid4x4Button.interactable = true;

        // Regenerate board
        GenerateBoard(rows, columns);
    }

    // UI Buttons
    public void SetGrid2x2() => GenerateBoard(2, 2);
    public void SetGrid3x4() => GenerateBoard(3, 4);
    public void SetGrid4x4() => GenerateBoard(4, 4);
}
