using UnityEngine;
using UnityEngine.UI;

public class CardGridScaler : MonoBehaviour
{
    public GridLayoutGroup gridLayout;
    public int rows = 2;
    public int columns = 2;
    public float spacing = 5f;

    void Start()
    {
        ScaleGrid();
    }

    public void ScaleGrid()
    {
        if (gridLayout == null) return;

        RectTransform rt = gridLayout.GetComponent<RectTransform>();
        float width = rt.rect.width - (spacing * (columns - 1));
        float height = rt.rect.height - (spacing * (rows - 1));

        float cellWidth = width / columns;
        float cellHeight = height / rows;

        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
        gridLayout.spacing = new Vector2(spacing, spacing);
    }

    public void SetGridSize(int newRows, int newColumns)
    {
        rows = newRows;
        columns = newColumns;
        ScaleGrid();
    }

    void OnRectTransformDimensionsChange()
    {
        ScaleGrid();
    }
}
