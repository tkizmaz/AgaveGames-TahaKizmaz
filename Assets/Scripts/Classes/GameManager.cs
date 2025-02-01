using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private int maxMoves = 10;
    [SerializeField] public int targetCount = 20;
    private TileColor targetColor;
    private int currentMoves;
    private int collectedTiles;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
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
            GameEvents.OnGameLost?.Invoke();
        }
    }
}
