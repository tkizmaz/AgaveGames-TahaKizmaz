using System.Collections.Generic;
using UnityEngine;

public class GridFlowManager
{
    private GridInfo gridInfo;
    private TileMovementManager tileMovementManager;

    public GridFlowManager(GridInfo gridInfo, TileMovementManager tileMovementManager)
    {
        this.gridInfo = gridInfo;
        this.tileMovementManager = tileMovementManager;
    }

    public void DropTiles(List<int> affectedColumns)
    {
        GameManager.Instance.ChangeState(GameState.TilesMoving);
        int rowCount = GameSettings.Instance.RowCount;
        Dictionary<int, int> emptySpacesPerColumn = new Dictionary<int, int>();

        foreach (int x in affectedColumns)
        {
            int lowestEmptyRow = -1;
            emptySpacesPerColumn[x] = 0;

            for (int y = rowCount - 1; y >= 0; y--)
            {
                Cell cell = gridInfo.Grid[x, y];

                if (!cell.IsOccupied)
                {
                    if (lowestEmptyRow == -1 || y > lowestEmptyRow)
                    {
                        lowestEmptyRow = y;
                    }
                    emptySpacesPerColumn[x]++;
                }
                else
                {
                    if (lowestEmptyRow != -1 && y < lowestEmptyRow)
                    {
                        Tile tile = cell.CurrentTile;
                        gridInfo.Grid[x, lowestEmptyRow].SetTile(tile);

                        if (tile is MovableTile movableTile)
                        {
                            tileMovementManager.RegisterMovingTile(movableTile, gridInfo.Grid[x, lowestEmptyRow]);
                        }

                        cell.ClearTileReference();
                        lowestEmptyRow--;
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
                GridManager.Instance.CreateTile(gridInfo.Grid[x, y]);
            }
        }
    }

}
