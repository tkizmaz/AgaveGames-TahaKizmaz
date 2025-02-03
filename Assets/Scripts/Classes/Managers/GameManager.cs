using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int maxMoves = 10;
    [SerializeField] public int targetCount = 20;
    private int currentMoves;
    private int collectedTiles;
    public GameState CurrentState { get; private set; }
    
    private TileData goalTileData; 

    private void Start()
    {
        InitializeGoal();
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

    private void InitializeGoal()
    {
        goalTileData = TileDatabase.Instance.GetRandomTileData();
        GameEvents.OnGoalTileChanged?.Invoke(goalTileData);
    }

    public void ResetGame()
    {
        currentMoves = maxMoves;
        collectedTiles = 0;

        InitializeGoal();

        ChangeState(GameState.WaitingForInput);

        GameEvents.OnMoveMade?.Invoke(currentMoves);
        GameEvents.OnGoalTileCountChanged?.Invoke(targetCount);
    }

    private void OnTileDestroyed(Tile tile)
    {
        if (tile == null || tile.TileData == null) return;

        bool isGoalTile = tile.TileData == goalTileData;

        if (isGoalTile)
        {
            collectedTiles++;
            GameEvents.OnGoalTileCountChanged?.Invoke(targetCount - collectedTiles);

            if (collectedTiles >= targetCount)
            {
                ChangeState(GameState.GameWon);
            }
        }
    }


    public void OnMoveMade()
    {
        if (currentMoves <= 0) return; 
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
