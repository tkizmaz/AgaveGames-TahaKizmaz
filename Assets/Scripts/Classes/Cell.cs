using UnityEngine;

public class Cell : MonoBehaviour
{
    Tile currentTile;
    private Vector2Int gridPosition;
    public Vector2Int GridPosition { get => gridPosition; set => gridPosition = value; }
    public Tile CurrentTile { get => currentTile; set => currentTile = value; }
    private bool isOccupied;
    public bool IsOccupied { get => isOccupied; set => isOccupied = value; }

    public void Initialize(Vector2Int position)
    {
        this.gridPosition = position;
    }

    public void SetTile(Tile tile)
    {
        currentTile = tile;   
        isOccupied = true;
    }

    public void RemoveCurrentTile()
    {
        if (currentTile == null)
        {
            Debug.LogError("Trying to remove a tile from an empty cell!");
            return;
        }
        GridManager.Instance.ReturnTileToPool(currentTile);
        ClearTileReference();
    }

    public void ClearTileReference()
    {
        currentTile = null;
        isOccupied = false;
    }
}
