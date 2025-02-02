using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : Singleton<GridManager>
{
    [SerializeField] private int rowCount;
    public int RowCount => rowCount;
    [SerializeField] private int columnCount;
    public int ColumnCount => columnCount;
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
    private int tilesMovingCount;

    protected override void Awake()
    {
        base.Awake();
        gridFlowManager = new GridFlowManager(this);
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

        moveValidator = new MoveValidator();
        gridShuffler = new GridShuffler();
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
        gridFlowManager.DropTiles(affectedColumns);
    }


    public Vector3 GetWorldPositionFromGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0 || gridPosition.x >= columnCount || gridPosition.y < 0 || gridPosition.y >= rowCount)
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
}
