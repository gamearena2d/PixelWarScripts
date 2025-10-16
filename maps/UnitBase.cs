using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float attackRange = 1f;
    public int damage = 1;
    public CellState owner = CellState.Player;

    private Cell currentCell;

    private void Update()
    {
        // esempio semplice: trova cella più vicina neutra o nemica e vai verso di essa
        if (currentCell == null)
            return;
    }

    public void SetCell(Cell cell)
    {
        currentCell = cell;
    }
}
