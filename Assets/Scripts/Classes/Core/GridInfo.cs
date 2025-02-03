using UnityEngine;

public class GridInfo
{
    public Cell[,] Grid { get; private set; }
    public int RowCount { get; private set; }
    public int ColumnCount { get; private set; }

    public GridInfo(Cell[,] grid, int rowCount, int columnCount)
    {
        this.Grid = grid;
        this.RowCount = rowCount;
        this.ColumnCount = columnCount;
    }
}
