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
        if (tile == null) return;
        currentTile = tile;   
        isOccupied = true;
        tile.transform.SetParent(transform);
    }

    public void RemoveCurrentTile()
    {
        if (currentTile == null) return;
        GridManager.Instance.ReturnTileToPool(currentTile);
        currentTile.transform.SetParent(null);
        ClearTileReference();
    }

    public void ClearTileReference()
    {
        currentTile = null;
        isOccupied = false;
    }
}
