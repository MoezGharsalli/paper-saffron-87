using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public float spacing = 1.5f;
    public List<Card> cards = new List<Card>();

    public void GenerateBoard(int rows, int columns)
    {
        // Clear old cards
        foreach (var c in cards)
            if (c != null) Destroy(c.gameObject);
        cards.Clear();

        // Calculate total board size
        float width = (columns - 1) * spacing;
        float height = (rows - 1) * spacing;

        Vector3 startPos = new Vector3(-width / 2f, height / 2f, 0);

        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 pos = startPos + new Vector3(c * spacing, -r * spacing, 0);
                GameObject cardObj = Instantiate(cardPrefab, pos, Quaternion.identity, transform);
                Card card = cardObj.GetComponent<Card>();
                card.id = (r * columns + c) % ((rows * columns) / 2);
                cards.Add(card);
            }
        }
    }
}
