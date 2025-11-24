using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public int rows = 2;
    public int columns = 2;
    public float spacing = 1.5f;

    private List<Card> cards = new List<Card>();

    void Start()
    {
        SetupBoard();
    }

    void SetupBoard()
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < columns; c++)
            {
                Vector3 pos = new Vector3(c * spacing, r * -spacing, 0);
                GameObject cardObj = Instantiate(cardPrefab, pos, Quaternion.identity, transform);
                Card card = cardObj.GetComponent<Card>();
                card.cardID = (r * columns + c) % ((rows * columns) / 2);
                cards.Add(card);
            }
        }
    }
}