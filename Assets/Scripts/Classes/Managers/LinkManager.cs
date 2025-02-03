using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour
{
    private List<Cell> linkedCells = new List<Cell>();
    private Camera mainCamera;
    private Cell lastHoveredCell;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        GameState gameState = GameManager.Instance.CurrentGameState;
        if(Input.GetMouseButtonDown(0) && gameState == GameState.WaitingForInput)
        {
            StartLinking();
        }
        else if(Input.GetMouseButton(0) && gameState == GameState.Linking)
        {
            TryAddTileToLink(); 
        }

        else if(Input.GetMouseButtonUp(0) && gameState == GameState.Linking)
        {
            FinishLinking();
        }
    }

    private void StartLinking()
    {
        linkedCells.Clear();
        Cell firstCell = GetSelectedCell();
        bool isSelectable = firstCell != null && firstCell.CurrentTile != null && firstCell.CurrentTile.TileData.selectableType == TileSelectableType.Selectable;
        if(isSelectable)
        {
            linkedCells.Add(firstCell);
            lastHoveredCell = firstCell;
            ChangeTileHighlight(firstCell.CurrentTile, true);
            GameManager.Instance.ChangeState(GameState.Linking);
        }
        else
        {
            GameManager.Instance.ChangeState(GameState.WaitingForInput);
        }
    }

    private Cell GetSelectedCell()
    {
        Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if(hit.collider != null)
        {
            Cell selectedCell = hit.collider.GetComponent<Cell>();
            if(selectedCell != null && selectedCell.CurrentTile != null)
            {
                return selectedCell;
            }
        }
        return null;
    }

    private void TryAddTileToLink()
    {
        Cell newCell = GetSelectedCell();
        if (newCell != null && newCell != lastHoveredCell)
        {
            lastHoveredCell = newCell;

            if (!linkedCells.Contains(newCell))
            {
                Cell lastCell = linkedCells[linkedCells.Count - 1];

                if (CheckIfTilesAreNeighbors(lastCell, newCell) && 
                    lastCell.CurrentTile.TileData == newCell.CurrentTile.TileData && 
                    newCell.CurrentTile.TileData.selectableType == TileSelectableType.Selectable)
                {
                    linkedCells.Add(newCell);
                    ChangeTileHighlight(newCell.CurrentTile, true);
                }
            }
        }
    }

    private void FinishLinking()
    {
        if(linkedCells.Count >= 3)
        {
            List<int> columnIndexes = new List<int>();
            foreach(Cell cell in linkedCells)
            {
                GameEvents.OnTileDestroyed?.Invoke(cell.CurrentTile);
                cell.RemoveCurrentTile();
                if(!columnIndexes.Contains(cell.GridPosition.x))
                {
                    columnIndexes.Add(cell.GridPosition.x);
                }
            }
            GridManager.Instance.DropTiles(columnIndexes);
            GameManager.Instance.OnMoveMade();
        }
        else
        {
            foreach(Cell cell in linkedCells)
            {
                ChangeTileHighlight(cell.CurrentTile, false);
            }
            GameManager.Instance.ChangeState(GameState.WaitingForInput);
        }
    }

    private bool CheckIfTilesAreNeighbors(Cell a, Cell b)
    {
        return Mathf.Abs(a.GridPosition.x - b.GridPosition.x) + Mathf.Abs(a.GridPosition.y - b.GridPosition.y) == 1;
    }

    private void ChangeTileHighlight(Tile tile, bool isSelected)
    {
        if(tile is ISelectable selectableTile)
        {
            selectableTile.SetSelected(isSelected);
        }
    }
}
