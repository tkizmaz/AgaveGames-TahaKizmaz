using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private int rowCount = 5;
    [SerializeField] private int columnCount = 5;
    [SerializeField] private GameObject cellPrefab; 
    [SerializeField] private GameObject tilePrefab; 
    [SerializeField] private float paddingPercentage = 0.05f;
    
    private Vector3 sizeModifier;
    private Cell[,] grid; 
    private ObjectPool<Tile> tilePool;
    private MoveValidator moveValidator;
    private GridShuffler gridShuffler;
    private int tilesMovingCount;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        tilePool = new ObjectPool<Tile>(tilePrefab.GetComponent<Tile>(), rowCount * columnCount, transform);
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        if (cellPrefab == null) return;

        grid = new Cell[rowCount, columnCount];

        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;
        float horizontalPadding = screenWidth * paddingPercentage;
        float verticalPadding = screenHeight * paddingPercentage;
        float usableWidth = screenWidth - (2 * horizontalPadding);
        float usableHeight = screenHeight - (2 * verticalPadding);
        float tileSize = Mathf.Min(usableWidth / columnCount, usableHeight / rowCount);

        Vector2 startPosition = new Vector2(
            -((columnCount * tileSize) / 2) + (tileSize / 2),
            ((rowCount * tileSize) / 2) - (tileSize / 2)
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

        moveValidator = new MoveValidator(grid, columnCount, rowCount);
        gridShuffler = new GridShuffler(grid, columnCount, rowCount);
        CheckForPossibleMoves();
    }

    private Cell InstantiateCell(Vector2Int gridPosition, Vector2 worldPosition, float tileSize)
    {
        GameObject cellObj = Instantiate(cellPrefab, worldPosition, Quaternion.identity);
        cellObj.transform.SetParent(transform);
        Cell cell = cellObj.GetComponent<Cell>();
        cell.Initialize(gridPosition);

        SpriteRenderer sr = cellObj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Vector2 spriteSize = sr.sprite.bounds.size;
            sizeModifier = new Vector3(tileSize / spriteSize.x, tileSize / spriteSize.y, 1);
            cellObj.transform.localScale = sizeModifier;
        }
        return cell;
    }

    public void DropTiles(List<int> affectedColumns)
    {
        GameManager.Instance.ChangeState(GameState.TilesMoving);
        tilesMovingCount = 0;

        foreach (int x in affectedColumns)
        {
            int lowestEmptyRow = -1;
            for (int y = rowCount - 1; y >= 0; y--)
            {
                Cell cell = grid[x, y];
                if (!cell.IsOccupied)
                {
                    if (lowestEmptyRow == -1 || y > lowestEmptyRow)
                    {
                        lowestEmptyRow = y;
                    }
                }
                else if (lowestEmptyRow != -1 && y < lowestEmptyRow)
                {
                    Tile tile = cell.CurrentTile;
                    grid[x, lowestEmptyRow].SetTile(tile);

                    if (tile is MovableTile movableTile)
                    {
                        movableTile.MoveToCell(grid[x, lowestEmptyRow], OnTileMovementComplete);
                        tilesMovingCount++;
                    }

                    cell.ClearTileReference();
                    lowestEmptyRow--;
                }
            }
        }
        RefillTiles(affectedColumns);
    }


    public Vector3 GetWorldPositionFromGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0 || gridPosition.x >= columnCount || gridPosition.y < 0 || gridPosition.y >= rowCount)
        {
            return Vector3.zero;
        }
        return grid[gridPosition.x, gridPosition.y].transform.position;
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

        if (tilesMovingCount == 0)
        {
            OnTileMovementComplete();
        }
    }

    public void ReturnTileToPool(Tile tile)
    {
        tile.gameObject.SetActive(false); 
        tilePool.ReturnToPool(tile);
    }


    private void CreateTile(Cell cell, bool isInitial = false)
    {
        Vector3 spawnPosition = isInitial ? cell.transform.position : new Vector3(cell.transform.position.x, cell.transform.position.y + 1.5f, 0);
        Tile tile = tilePool.GetFromPool();
        tile.transform.position = spawnPosition;
        tile.transform.localScale = sizeModifier;
        cell.SetTile(tile);
        tile.SpawnObject();

        if (!isInitial && tile is MovableTile movableTile)
        {
            tilesMovingCount++;
            movableTile.MoveToCell(cell, OnTileMovementComplete);
        }
    }

    private void OnTileMovementComplete()
    {
        tilesMovingCount--;

        if (tilesMovingCount == 0)
        {
            CheckForPossibleMoves();
        }
    }

    public void CheckForPossibleMoves()
    {
        if (!moveValidator.HasAvailableMoves())
        {
            StartCoroutine(ShuffleUntilValidMoves());
        }
        else
        {
            GameManager.Instance.ChangeState(GameState.WaitingForInput);
        }
    }

    private IEnumerator ShuffleUntilValidMoves()
    {
        while (!moveValidator.HasAvailableMoves())
        {
            GameManager.Instance.ChangeState(GameState.Shuffling);
            gridShuffler.ShuffleBoard();
            yield return new WaitForSeconds(0.5f);
        }
        GameManager.Instance.ChangeState(GameState.WaitingForInput);
    }
}
