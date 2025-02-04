using System.Collections.Generic;
using UnityEngine;

public class MoveValidator
{
    private GridInfo gridInfo;
    public MoveValidator(GridInfo gridInfo)
    {
        this.gridInfo = gridInfo;
    }

    public bool HasAvailableMoves()
    {
        bool[,] visited = new bool[gridInfo.ColumnCount, gridInfo.RowCount];
        List<Vector2Int> tileCheckOrder = GetTileCheckOrder();

        foreach (Vector2Int pos in tileCheckOrder)
        {
            int x = pos.x;
            int y = pos.y;

            if (!gridInfo.Grid[x, y].IsOccupied) continue;

            Tile tile = gridInfo.Grid[x, y].CurrentTile;
            
            if (!(tile.TileData is ChipData chipData)) continue;

            List<Cell> connectedTiles = new List<Cell>();
            FindConnectedTiles(x, y, chipData, connectedTiles, visited);

            if (connectedTiles.Count >= 3) return true;
        }

        return false;
    }

    private void FindConnectedTiles(int x, int y, ChipData referenceChip, List<Cell> connectedTiles, bool[,] visited)
    {
        if (x < 0 || x >= gridInfo.ColumnCount || y < 0 || y >= gridInfo.RowCount || visited[x, y]) return;

        Cell cell = gridInfo.Grid[x, y];
        if (!cell.IsOccupied || cell.CurrentTile.TileData != referenceChip) return;

        visited[x, y] = true;
        connectedTiles.Add(cell);

        FindConnectedTiles(x + 1, y, referenceChip, connectedTiles, visited);
        FindConnectedTiles(x - 1, y, referenceChip, connectedTiles, visited);
        FindConnectedTiles(x, y + 1, referenceChip, connectedTiles, visited);
        FindConnectedTiles(x, y - 1, referenceChip, connectedTiles, visited);
    }

    private List<Vector2Int> GetTileCheckOrder()
    {
        List<Vector2Int> positions = new List<Vector2Int>();

        int centerX = gridInfo.ColumnCount / 2;
        int centerY = gridInfo.RowCount / 2;

        for (int x = 0; x < gridInfo.ColumnCount; x++)
        {
            for (int y = 0; y < gridInfo.RowCount; y++)
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
