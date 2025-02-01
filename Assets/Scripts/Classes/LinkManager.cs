using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkManager : MonoBehaviour
{
    private List<Cell> linkedCells = new List<Cell>();
    private bool isLinking = false;
    private Camera mainCamera;
    private Cell lastHoveredCell;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartLinking();
        }
        else if(Input.GetMouseButton(0) && isLinking)
        {
            TryAddTileToLink(); 
        }

        else if(Input.GetMouseButtonUp(0) && isLinking)
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
            isLinking = true;
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
                }
            }
        }
    }

    private void FinishLinking()
    {
        isLinking = false;
        if(linkedCells.Count >= 3)
        {
            List<int> columnIndexes = new List<int>();
            foreach(Cell cell in linkedCells)
            {
                cell.RemoveCurrentTile();
                if(!columnIndexes.Contains(cell.GridPosition.x))
                {
                    columnIndexes.Add(cell.GridPosition.x);
                }
            }

            GridManager.Instance.DropTiles(columnIndexes);
        }
        else
        {
            Debug.Log("Link is too short");
        }
    }

    private bool CheckIfTilesAreNeighbors(Cell a, Cell b)
    {
        return (Mathf.Abs(a.GridPosition.x - b.GridPosition.x) == 1 && a.GridPosition.y == b.GridPosition.y) || (Mathf.Abs(a.GridPosition.y - b.GridPosition.y) == 1 && a.GridPosition.x == b.GridPosition.x);
    }
}
