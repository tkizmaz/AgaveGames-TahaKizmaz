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
        GameState gameState = GameManager.Instance.CurrentState;
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

        if(firstCell != null)
        {
            linkedCells.Add(firstCell);
            lastHoveredCell = firstCell;
            firstCell.CurrentTile.SetSelected(true);
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
        if(newCell != null && newCell != lastHoveredCell)
        {
            lastHoveredCell = newCell;

            if(!linkedCells.Contains(newCell))
            {
                Cell lastCell = linkedCells[linkedCells.Count - 1];
                if(CheckIfTilesAreNeighbors(lastCell, newCell) && lastCell.CurrentTile.TileData.tileColor == newCell.CurrentTile.TileData.tileColor)
                {
                    linkedCells.Add(newCell);
                    newCell.CurrentTile.SetSelected(true);
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
                cell.CurrentTile.SetSelected(false);
            }
            GameManager.Instance.ChangeState(GameState.WaitingForInput);
        }
    }

    private bool CheckIfTilesAreNeighbors(Cell a, Cell b)
    {
        return (Mathf.Abs(a.GridPosition.x - b.GridPosition.x) == 1 && a.GridPosition.y == b.GridPosition.y) || (Mathf.Abs(a.GridPosition.y - b.GridPosition.y) == 1 && a.GridPosition.x == b.GridPosition.x);
    }
}
