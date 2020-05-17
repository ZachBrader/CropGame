using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Farm : MonoBehaviour
{
    // Reference to the cell prefab for creating new cells
    public GameObject mCellPrefab;

    // Width and height of the farm
    public int mCellWidth;
    public int mCellHeight;

    // Holds all the cells available in grid
    [HideInInspector]
    public Cell[,] mAllCells = null;

    // Start is called before the first frame update
    void Start()
    {
        mAllCells = new Cell[mCellWidth + 2, mCellHeight + 2];

        for (int y = 0; y < mCellHeight + 2; y++)
        {
            for (int x = 0; x < mCellWidth + 2; x++)
            {
                // Create Cell
                GameObject newCell = Instantiate(mCellPrefab, transform);

                // Set position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);

                // Instantiate cell
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2(x, y), this);
            }
        }

        for (int y = 0; y < mCellHeight + 2; y++)
        {
            mAllCells[0, y].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            mAllCells[mCellWidth + 1, y].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
            if (y == 0 || y == mCellHeight + 1)
            {
                for (int x = 0; x < mCellWidth + 2; x++)
                {
                    mAllCells[x, y].GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                }
            }
        }

        for (int y = 1; y < mCellHeight + 1; y++)
        {
            for (int x = 1; x < mCellWidth + 1; x += 2)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalX = x + offset;
                mAllCells[finalX, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
