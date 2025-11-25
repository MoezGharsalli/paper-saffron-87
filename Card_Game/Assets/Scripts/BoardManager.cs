using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public CardGridScaler gridScaler;

    public Sprite[] cardFrontImages;

    [HideInInspector] public List<Card> cards = new List<Card>();

    public void GenerateBoard(int rows, int columns)
    {
        if (gridScaler != null)
            gridScaler.SetGridSize(rows, columns);

        // Clear old cards
        foreach (var c in cards)
            if (c != null) Destroy(c.gameObject);
        cards.Clear();

        int totalCards = rows * columns;
        int pairCount = totalCards / 2;

        // Create list of image indices (two of each for pairs)
        List<int> imageIndices = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            imageIndices.Add(i);
            imageIndices.Add(i);
        }

        // Shuffle the image indices
        Shuffle(imageIndices);

        // Instantiate cards
        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, gridScaler.gridLayout.transform);
            Card card = cardObj.GetComponent<Card>();
            card.id = imageIndices[i]; // assign pair ID
            card.frontSprite = cardFrontImages[imageIndices[i]]; // assign front image
            cards.Add(card);
        }
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
