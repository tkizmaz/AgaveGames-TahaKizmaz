using System.Collections.Generic;
using UnityEngine;

public class MoveValidator
{
    private GridManager gridManager;

    public MoveValidator()
    {
        this.gridManager = GridManager.Instance;
    }

    public bool HasAvailableMoves()
    {
        Cell[,] grid = gridManager.Grid;
        int columnCount = gridManager.ColumnCount;
        int rowCount = gridManager.RowCount;
        
        bool[,] visited = new bool[columnCount, rowCount];
        List<Vector2Int> tileCheckOrder = GetTileCheckOrder();

        foreach (Vector2Int pos in tileCheckOrder)
        {
            int x = pos.x;
            int y = pos.y;

            if (!grid[x, y].IsOccupied) continue;

            Tile tile = grid[x, y].CurrentTile;
            TileColor color = tile.TileData.tileColor;

            List<Cell> connectedTiles = new List<Cell>();
            FindConnectedTiles(x, y, color, connectedTiles, visited);

            if (connectedTiles.Count >= 3) return true;
        }
        return false;
    }

    private void FindConnectedTiles(int x, int y, TileColor color, List<Cell> connectedTiles, bool[,] visited)
    {
        Cell[,] grid = gridManager.Grid;
        int columnCount = gridManager.ColumnCount;
        int rowCount = gridManager.RowCount;

        if (x < 0 || x >= columnCount || y < 0 || y >= rowCount || visited[x, y]) return;

        Cell cell = grid[x, y];
        if (!cell.IsOccupied || cell.CurrentTile.TileData.tileColor != color) return;

        visited[x, y] = true;
        connectedTiles.Add(cell);

        FindConnectedTiles(x + 1, y, color, connectedTiles, visited);
        FindConnectedTiles(x - 1, y, color, connectedTiles, visited);
        FindConnectedTiles(x, y + 1, color, connectedTiles, visited);
        FindConnectedTiles(x, y - 1, color, connectedTiles, visited);
    }

    private List<Vector2Int> GetTileCheckOrder()
    {
        int columnCount = gridManager.ColumnCount;
        int rowCount = gridManager.RowCount;

        List<Vector2Int> positions = new List<Vector2Int>();

        int centerX = columnCount / 2;
        int centerY = rowCount / 2;

        for (int x = 0; x < columnCount; x++)
        {
            for (int y = 0; y < rowCount; y++)
            {
                positions.Add(new Vector2Int(x, y));
            }
        }

        positions.Sort((a, b) =>
        {
            int distA = Mathf.Abs(a.x - centerX) + Mathf.Abs(a.y - centerY);
            int distB = Mathf.Abs(b.x - centerX) + Mathf.Abs(b.y - centerY);
            return distA.CompareTo(distB);
        });

        return positions;
    }
}
