using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager instance;
    public static GridManager Instance { get => instance; }
    [SerializeField] private int rowCount = 5;
    [SerializeField] private int columnCount = 5;
    [SerializeField] private GameObject cellPrefab; 
    [SerializeField] private GameObject tilePrefab; 
    [SerializeField] private float paddingPercentage = 0.05f;
    private Vector3 sizeModifier;
    private Cell[,] grid; 
    ObjectPool<Tile> tilePool;

    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        tilePool = new ObjectPool<Tile>(tilePrefab.GetComponent<Tile>(), rowCount * columnCount, transform);
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        if (cellPrefab == null)
        {
            return;
        }

        grid = new Cell[rowCount, columnCount];

        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;

        float horizontalPadding = screenWidth * paddingPercentage;
        float verticalPadding = screenHeight * paddingPercentage;

        float usableWidth = screenWidth - (2 * horizontalPadding);
        float usableHeight = screenHeight - (2 * verticalPadding);

        float tileWidth = usableWidth / columnCount;
        float tileHeight = usableHeight / rowCount;
        float tileSize = Mathf.Min(tileWidth, tileHeight);

        float totalGridWidth = columnCount * tileSize;
        float totalGridHeight = rowCount * tileSize;

        Vector2 startPosition = new Vector2(
            -totalGridWidth / 2 + tileSize / 2, 
            totalGridHeight / 2 - tileSize / 2   
        );

        for (int x = 0; x < columnCount; x++)
        {
            for (int y = 0; y < rowCount; y++)
            {
                Vector2 worldPosition = startPosition + new Vector2(x * tileSize, -y * tileSize);
                Cell cell = InstantiateCell(new Vector2Int(x, y), worldPosition, tileSize);
                grid[x, y] = cell;

                CreateTile(cell, true);
            }
        }
    }

    private Cell InstantiateCell(Vector2Int gridPosition, Vector2 worldPosition, float tileSize)
    {
        GameObject cellObj = Instantiate(cellPrefab, worldPosition, Quaternion.identity);
        Cell cell = cellObj.GetComponent<Cell>();

        if (cell != null)
        {
            cell.Initialize(gridPosition);
        }

        SpriteRenderer sr = cellObj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Vector2 spriteSize = sr.sprite.bounds.size;
            sizeModifier = new Vector3(tileSize / spriteSize.x, tileSize / spriteSize.y, 1);
            cellObj.transform.localScale = sizeModifier;
        }
        return cell;
    }

    public Vector3 GetWorldPositionFromGridPosition(Vector2Int gridPosition)
    {
        return grid[gridPosition.x, gridPosition.y].transform.position;
    }

    public void DropTiles(List<int> affectedColumns)
    {
        foreach (int x in affectedColumns)
        {
            int lowestEmptyRow = -1;
            for(int y = rowCount -1; y >= 0; y--)
            {
                Cell cell = grid[x,y];
                if(!cell.IsOccupied)
                {
                    if(lowestEmptyRow == -1 || y > lowestEmptyRow)
                    {
                        lowestEmptyRow = y;
                    }
                }
                else if(lowestEmptyRow != -1 &&  y < lowestEmptyRow)
                {
                    Tile tile = cell.CurrentTile;
                
                    grid[x, lowestEmptyRow].SetTile(tile);
                    tile.MoveToCell(grid[x, lowestEmptyRow]);
                    cell.ClearTileReference();

                    lowestEmptyRow--;
                }
            }
        }

        RefillTiles(affectedColumns);
    }

    private void RefillTiles(List<int> columnsToBeCreated)
    {
        foreach (int x in columnsToBeCreated)
        {
            for (int y = 0; y < rowCount; y++)
            {
                Cell cell = grid[x, y];
                if (!cell.IsOccupied)
                {
                    CreateTile(cell);
                }
            }
        }
    }


    private void CreateTile(Cell cell, bool isInitial = false)
    {
        Vector3 spawnPosition = isInitial ? cell.transform.position : new Vector3(cell.transform.position.x, cell.transform.position.y + 1.5f, 0);
        Tile tile = tilePool.GetFromPool();
        tile.transform.position = spawnPosition;
        tile.transform.localScale = sizeModifier;

        cell.SetTile(tile);
        tile.SpawnObject();
        
        if (!isInitial)
        {
            tile.MoveToCell(cell);
        }
    }

    public void ReturnTileToPool(Tile tile)
    {
        tilePool.ReturnToPool(tile);
    }
}





