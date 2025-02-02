using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int maxMoves = 10;
    [SerializeField] public int targetCount = 20;
    private int currentMoves;
    private int collectedTiles;
    public GameState CurrentState { get; private set; }
    
    private TileData targetTileData; 

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitializeTarget();
        ResetGame();
        ChangeState(GameState.WaitingForInput);
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
        targetTileData = TileDatabase.Instance.GetRandomTileData();
        GameEvents.OnTargetTileChanged?.Invoke(targetTileData);
    }

    public void ResetGame()
    {
        currentMoves = maxMoves;
        collectedTiles = 0;

        GameEvents.OnMoveMade?.Invoke(currentMoves);
        GameEvents.OnTargetTileCountChanged?.Invoke(targetCount);
    }

    private void OnTileDestroyed(Tile tile)
    {
        if (tile == null || tile.TileData == null) return;

        if (tile.TileData == targetTileData) 
        {
            collectedTiles++;
            GameEvents.OnTargetTileCountChanged?.Invoke(targetCount - collectedTiles);

            if (collectedTiles >= targetCount)
            {
                ChangeState(GameState.GameWon);
            }
        }
    }

    public void OnMoveMade()
    {
        currentMoves--;

        GameEvents.OnMoveMade?.Invoke(currentMoves);
        if (currentMoves <= 0)
        {
            ChangeState(GameState.GameOver);
        }
    }

    public void ChangeState(GameState newState)
    {
        if (CurrentState == newState || CurrentState == GameState.GameWon || CurrentState == GameState.GameOver) return;
        
        CurrentState = newState;
        GameEvents.OnGameStateChanged?.Invoke(newState);
    }
}
