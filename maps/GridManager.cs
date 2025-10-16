using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width = 8;
    public int height = 12;
    public Cell cellPrefab;
    private Cell[,] grid;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new Cell[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                grid[x, y] = cell;
            }
        }
    }

    public List<Cell> GetCellsInRange(Vector3 pos, float radius)
    {
        List<Cell> cellsInRange = new List<Cell>();
        // codice per trovare celle in range
        return cellsInRange;
    }

    public void PaintCell(Cell cell, CellState state, float intensity)
    {
        if (cell != null)
            cell.SetState(state, intensity);
    }
}
