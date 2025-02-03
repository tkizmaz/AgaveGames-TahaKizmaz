using UnityEngine;

public class TileMovementManager
{
    private int tilesMovingCount;

    public void RegisterMovingTile(MovableTile tile, Cell targetCell)
    {
        tilesMovingCount++;
        tile.MoveToCell(targetCell, OnTileMovementComplete);
    }
    
    private void OnTileMovementComplete()
    {
        tilesMovingCount--;
        CheckForEndOfMovement();
    }

    public bool HasMovingTiles()
    {
        return tilesMovingCount > 0;
    }

    private void CheckForEndOfMovement()
    {
        if (tilesMovingCount <= 0)
        {
            tilesMovingCount = 0; 
            GridManager.Instance.CheckForPossibleMoves();
        }
    }
}
