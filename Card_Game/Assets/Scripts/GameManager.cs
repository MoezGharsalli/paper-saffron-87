using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardManager board;
    public ScoreManager scoreManager;
    public GameObject victoryPanel;
    private bool comparingQueue = false;

    public int rows = 2;
    public int columns = 2;

    private List<Card> flippedQueue = new List<Card>();
    private List<Card> selected = new List<Card>();
    private bool comparing = false;

    public int score { get; set; } = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateBoard(rows, columns);
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
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
        if (card.IsFaceUp || card.IsMatched) return;

        card.Flip(true);

        // Add to queue for comparison
        flippedQueue.Add(card);

        // Start processing only if not already processing
        if (!comparingQueue)
            StartCoroutine(ProcessQueue());
    }


    IEnumerator CompareRoutine()
    {
        if (selected.Count < 2) yield break;

        Card a = selected[0];
        Card b = selected[1];

        yield return new WaitForSeconds(0.25f); // small delay so player sees flips

        if (a.id == b.id)
        {
            a.MarkMatched();
            b.MarkMatched();
            score += 10;

            a.gameObject.SetActive(false);
            b.gameObject.SetActive(false);
        }
        else
        {
            score -= 1;
            yield return new WaitForSeconds(0.4f);
            a.Flip(false);
            b.Flip(false);
        }

        // Remove these two from selected
        selected.Remove(a);
        selected.Remove(b);

        // Repeat if more than 2 cards were flipped quickly
        if (selected.Count >= 2)
        {
            StartCoroutine(CompareRoutine());
        }
    }

    private IEnumerator ProcessQueue()
    {
        comparingQueue = true;

        while (flippedQueue.Count >= 2)
        {
            Card a = flippedQueue[0];
            Card b = flippedQueue[1];

            // Small delay so player sees flips
            yield return new WaitForSeconds(0.25f);

            if (a.id == b.id)
            {
                a.MarkMatched();
                b.MarkMatched();
                score += 10;
                a.gameObject.SetActive(false);
                b.gameObject.SetActive(false);
            }
            else
            {
                score -= 1;
                yield return new WaitForSeconds(0.4f);
                a.Flip(false);
                b.Flip(false);
            }

            // Remove processed cards
            flippedQueue.Remove(a);
            flippedQueue.Remove(b);
        }

        comparingQueue = false;

        // Check victory
        CheckVictory();
    }

    private void CheckVictory()
    {
        // Make sure all cards in board.cards exist and are matched
        foreach (var card in board.cards)
        {
            if (card != null && !card.IsMatched)
            {
                return; // exit early if any unmatched card found
            }
        }

        // If we reach here, all cards are matched
        Victory();
    }

    private void Victory()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true); // show the panel
        }

        Debug.Log("Victory!");
    }

    public void RestartGame()
    {
        // Hide victory panel before generating new board
        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        GenerateBoard(rows, columns);
    }

    // Optional UI buttons
    public void SetGrid2x2() => GenerateBoard(2, 2);
    public void SetGrid3x4() => GenerateBoard(3, 4);
    public void SetGrid4x4() => GenerateBoard(4, 4);
}
