using System.Collections.Generic;
using UnityEngine;

public class StructureBase : MonoBehaviour
{
    public float attackRange = 3f;
    public int damage = 1;
    public float paintIntensity = 0.2f;
    public CellState owner = CellState.Player;

    public void Attack(GridManager grid)
    {
        List<Cell> cells = grid.GetCellsInRange(transform.position, attackRange);
        foreach (var cell in cells)
        {
            grid.PaintCell(cell, owner, paintIntensity);
        }
    }
}
