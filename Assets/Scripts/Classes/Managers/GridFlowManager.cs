using System.Collections.Generic;
using UnityEngine;

public class GridFlowManager
{
    private GridManager gridManager;
    private int tilesMovingCount;

    public GridFlowManager(GridManager gridManager)
    {
        this.gridManager = gridManager;
    }

    public void DropTiles(List<int> affectedColumns)
    {
        GameManager.Instance.ChangeState(GameState.TilesMoving);
        tilesMovingCount = 0;

        Dictionary<int, int> emptySpacesPerColumn = new Dictionary<int, int>();

        foreach (int x in affectedColumns)
        {
            int lowestEmptyRow = -1;
            bool foundImmovable = false;
            emptySpacesPerColumn[x] = 0;

            for (int y = gridManager.RowCount - 1; y >= 0; y--)
            {
                Cell cell = gridManager.Grid[x, y];

                if (!cell.IsOccupied)
                {
                    if (lowestEmptyRow == -1 || y > lowestEmptyRow)
                    {
                        lowestEmptyRow = y;
                    }
                    emptySpacesPerColumn[x]++; // Boşluk sayısını arttır
                }
                else
                {
                    Tile tile = cell.CurrentTile;

                    if (tile.TileData.moveType == TileMoveType.Immovable)
                    {
                        foundImmovable = true;
                        lowestEmptyRow = -1;
                        continue;
                    }

                    if (lowestEmptyRow != -1 && y < lowestEmptyRow)
                    {
                        gridManager.Grid[x, lowestEmptyRow].SetTile(tile);

                        if (tile is MovableTile movableTile)
                        {
                            movableTile.MoveToCell(gridManager.Grid[x, lowestEmptyRow], OnTileMovementComplete);
                            tilesMovingCount++;
                        }

                        cell.ClearTileReference();
                        lowestEmptyRow--;
                    }

                    if (foundImmovable && tile.TileData.moveType == TileMoveType.Movable)
                    {
                        lowestEmptyRow = y;
                    }
                }
            }
        }

        RefillTiles(emptySpacesPerColumn);
    }

    private void RefillTiles(Dictionary<int, int> emptySpacesPerColumn)
    {
        foreach (var columnData in emptySpacesPerColumn)
        {
            int x = columnData.Key;
            int emptySpaces = columnData.Value;

            for (int i = 0; i < emptySpaces; i++)
            {
                int y = i;

                gridManager.CreateTile(gridManager.Grid[x, y]); 
            }
        }

        if (tilesMovingCount == 0)
        {
            OnTileMovementComplete();
        }
    }

    private void OnTileMovementComplete()
    {
        tilesMovingCount--;

        if (tilesMovingCount == 0)
        {
            gridManager.CheckForPossibleMoves();
        }
    }
}
