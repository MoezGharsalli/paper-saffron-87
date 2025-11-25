using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

    private Queue<Card> compareQueue = new Queue<Card>();
    private bool isComparing = false;

    private void Awake() { Instance = this; }

    private void Start()
    {
        comboCount = 0;
        GenerateBoard(rows, columns);
    }

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
        if (card.State != CardState.FaceDown) return;

        card.Flip(true, () =>
        {
            compareQueue.Enqueue(card);
            TryCompareNext();
        });
    }

    private IEnumerator CompareRoutine(Card a, Card b)
    {
        isComparing = true;
        yield return new WaitForSeconds(0.25f);

        if (a.id == b.id)
        {
            a.MarkMatched();
            b.MarkMatched();
            AudioManager.Instance?.PlayMatch();

            comboCount++;
            int points = 10;
            if (comboCount > 1)
                AudioManager.Instance?.PlayCombo();
            points += (comboCount - 1) * comboBonusPerChain;
            score += points;
            


            if (scoreManager != null && scoreManager.comboText != null)
                StartCoroutine(scoreManager.PunchCombo());
        }
        else
        {
            comboCount = 0;
            score -= 2;
            yield return new WaitForSeconds(0.4f);
            a.Flip(false);
            b.Flip(false);
        }

        isComparing = false;

        bool allMatched = true;
        foreach (var card in board.cards)
            if (card.State != CardState.Matched) { allMatched = false; break; }

        if (allMatched) Victory();

        TryCompareNext();
    }

    private void TryCompareNext()
    {
        if (isComparing || compareQueue.Count < 2)
            return;

        Card a = compareQueue.Dequeue();
        Card b = compareQueue.Dequeue();

        StartCoroutine(CompareRoutine(a, b));
    }

    void Victory()
    {

        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        if (grid2x2Button != null) grid2x2Button.interactable = false;
        if (grid3x4Button != null) grid3x4Button.interactable = false;
        if (grid4x4Button != null) grid4x4Button.interactable = false;

        AudioManager.Instance?.PlayVictory();

        Debug.Log("Victory!");
    }

    public void ResetScore()
    {
        score = 0;
    }

    public void RestartGame()
    {
        comboCount = 0;
        bonusScore = 0;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        if (grid2x2Button != null) grid2x2Button.interactable = true;
        if (grid3x4Button != null) grid3x4Button.interactable = true;
        if (grid4x4Button != null) grid4x4Button.interactable = true;

        GenerateBoard(rows, columns);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SetGrid2x2() => GenerateBoard(2, 2);
    public void SetGrid3x4() => GenerateBoard(3, 4);
    public void SetGrid4x4() => GenerateBoard(4, 4);
}
