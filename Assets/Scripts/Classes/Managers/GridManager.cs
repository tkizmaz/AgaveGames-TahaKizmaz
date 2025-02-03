using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private GameObject cellPrefab; 
    [SerializeField] private GameObject tilePrefab; 
    [SerializeField] private float paddingPercentage = 0.05f;
    private Vector3 sizeModifier;
    private Cell[,] grid; 
    public Cell[,] Grid => grid;
    private ObjectPool<Tile> tilePool;
    private MoveValidator moveValidator;
    private GridShuffler gridShuffler;
    private GridFlowManager gridFlowManager;
    private GameSettings gameSettings;

    void Start()
    {
        gameSettings = GameSettings.Instance;
        tilePool = new ObjectPool<Tile>(tilePrefab.GetComponent<Tile>(), gameSettings.RowCount * gameSettings.ColumnCount, transform);
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        if (cellPrefab == null) return;

        grid = new Cell[gameSettings.RowCount, gameSettings.ColumnCount];

        float tileSize = CalculateTileSize();
        Vector2 gridOffset = GetGridOffset(tileSize);

        for (int x = 0; x < gameSettings.ColumnCount; x++)
        {
            for (int y = 0; y < gameSettings.RowCount; y++)
            {
                Vector2 worldPosition = gridOffset + new Vector2(x * tileSize, -y * tileSize);
                Cell cell = InstantiateCell(new Vector2Int(x, y), worldPosition, tileSize);
                grid[x, y] = cell;
                CreateTile(cell, true);
            }
        }

        InitializeManagers();
        CheckForPossibleMoves();
    }

    private float CalculateTileSize()
    {
        float screenHeight = Camera.main.orthographicSize * 2f;
        float screenWidth = screenHeight * Camera.main.aspect;
        float usableWidth = screenWidth * (1f - 2 * paddingPercentage);
        float usableHeight = screenHeight * (1f - 2 * paddingPercentage);
        return Mathf.Min(usableWidth / gameSettings.ColumnCount, usableHeight / gameSettings.RowCount);
    }

    private Vector2 GetGridOffset(float tileSize)
    {
        return new Vector2(
            -((gameSettings.ColumnCount * tileSize) / 2) + (tileSize / 2),
            ((gameSettings.RowCount * tileSize) / 2) - (tileSize / 2)
        );
    }

    private Cell InstantiateCell(Vector2Int gridPosition, Vector2 worldPosition, float tileSize)
    {
        GameObject cellObj = Instantiate(cellPrefab, worldPosition, Quaternion.identity, transform);
        Cell cell = cellObj.GetComponent<Cell>();
        cell.Initialize(gridPosition);

        SpriteRenderer sr = cellObj.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sizeModifier = Vector3.one * (tileSize / sr.sprite.bounds.size.x);
            cellObj.transform.localScale = sizeModifier;
        }
        return cell;
    }

    public void DropTiles(List<int> affectedColumns)
    {
        gridFlowManager.DropTiles(affectedColumns);
    }

    public Vector3 GetWorldPositionFromGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0 || gridPosition.x >= gameSettings.ColumnCount || gridPosition.y < 0 || gridPosition.y >= gameSettings.RowCount)
        {
            return Vector3.zero;
        }
        return grid[gridPosition.x, gridPosition.y].transform.position;
    }

    public void CreateTile(Cell cell, bool isInitial = false)
    {
        Vector3 spawnPosition = isInitial ? cell.transform.position : new Vector3(cell.transform.position.x, cell.transform.position.y + 1.5f, 0);

        TileData tileData = TileDatabase.Instance.GetRandomTileData();
        Tile tile = tilePool.GetFromPool();
        tile = TileFactory.CreateTile(tile, tileData, transform);

        tile.transform.position = spawnPosition;
        tile.transform.localScale = sizeModifier;
        cell.SetTile(tile);

        if (!isInitial && tile is MovableTile movableTile)
        {
            gridFlowManager.RegisterMovingTile(movableTile, cell);
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

    public void ReturnTileToPool(Tile tile)
    {
        tile.gameObject.SetActive(false);
        tilePool.ReturnToPool(tile);
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

    public void ResetGrid()
    {
        foreach (var cell in grid)
        {
            if (cell.IsOccupied)
            {
                cell.RemoveCurrentTile();  
            }

            CreateTile(cell, true);
        }

        CheckForPossibleMoves();
    }

    private void InitializeManagers()
    {
        moveValidator = new MoveValidator();
        gridShuffler = new GridShuffler();
        gridFlowManager = new GridFlowManager(this);
    }

}
