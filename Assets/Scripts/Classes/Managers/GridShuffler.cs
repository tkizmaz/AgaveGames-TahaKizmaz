using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GridShuffler
{
    private Cell[,] grid;
    private int columnCount;
    private int rowCount;

    public GridShuffler(Cell[,] grid, int columns, int rows)
    {
        this.grid = grid;
        this.columnCount = columns;
        this.rowCount = rows;
    }

    public void ShuffleBoard()
    {
        List<Tile> allTiles = new List<Tile>();
        for (int x = 0; x < columnCount; x++)
        {
            for (int y = 0; y < rowCount; y++)
            {
                if (grid[x, y].IsOccupied)
                {
                    allTiles.Add(grid[x, y].CurrentTile);
                }
            }
        }

        int n = allTiles.Count;
        for (int i = n - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (allTiles[i], allTiles[randomIndex]) = (allTiles[randomIndex], allTiles[i]);
        }

        int index = 0;
        for (int x = 0; x < columnCount; x++)
        {
            for (int y = 0; y < rowCount; y++)
            {
                if (grid[x, y].IsOccupied)
                {
                    Tile tile = allTiles[index++];
                    if (tile is MovableTile movableTile)
                    {
                        movableTile.MoveToCell(grid[x, y]);
                    }
                    grid[x, y].SetTile(tile);
                }
            }
        }
    }
}
