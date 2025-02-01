using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int maxMoves = 10;
    [SerializeField] public int targetCount = 20;
    private TileColor targetColor;
    private int currentMoves;
    private int collectedTiles;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeTarget();
        ResetGame();
    }

    private void OnEnable()
    {
        GameEvents.OnTileDestroyed += OnTileDestroyed;
    }

    private void OnDisable()
    {
        GameEvents.OnTileDestroyed -= OnTileDestroyed;
    }

    private void InitializeTarget()
    {
        targetColor = (TileColor)Random.Range(0, System.Enum.GetNames(typeof(TileColor)).Length);
    }

    public void ResetGame()
    {
        currentMoves = maxMoves;
        collectedTiles = 0;

        GameEvents.OnMoveMade?.Invoke(currentMoves);
        GameEvents.OnTargetTileChanged?.Invoke(targetColor); 
        GameEvents.OnTargetTileCountChanged?.Invoke(targetCount);
    }

    private void OnTileDestroyed(Tile tile)
    {
        if (tile == null || tile.TileData == null)
        {
            return;
        }

        if (tile.TileData.tileColor == targetColor)
        {
            collectedTiles++;
            GameEvents.OnTargetTileCountChanged?.Invoke(targetCount - collectedTiles);

            if (collectedTiles >= targetCount)
            {
                Debug.Log("Game Won!");
                GameEvents.OnGameWon?.Invoke();
            }
        }
    }

    public void OnMoveMade()
    {
        currentMoves--;

        GameEvents.OnMoveMade?.Invoke(currentMoves);
        if (currentMoves <= 0)
        {
            Debug.Log("Game Lost!");
            GameEvents.OnGameLost?.Invoke();
        }
    }
}
