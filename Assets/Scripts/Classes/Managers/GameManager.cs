using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int currentMoves;
    private int collectedTiles;
    public GameState CurrentState { get; private set; }
    private TileData goalTileData; 
    private bool isFirstGame = true;
    private GameSettings gameSettings;

    private void Start()
    {
        gameSettings = GameSettings.Instance;
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

    private void InitializeGoal()
    {
        goalTileData = TileDatabase.Instance.GetRandomTileData();
        GameEvents.OnGoalTileChanged?.Invoke(goalTileData);
    }

    public void ResetGame()
    {
        currentMoves = gameSettings.MaxMoves;
        collectedTiles = 0;

        InitializeGoal();

        if(isFirstGame) isFirstGame = false;
        else
        {
            GridManager.Instance.ResetGrid();
        }

        GameEvents.OnMoveMade?.Invoke(currentMoves);
        GameEvents.OnGoalTileCountChanged?.Invoke(gameSettings.GoalTileCount);
        CurrentState = GameState.WaitingForInput;
        UIManager.Instance.ResetUI();
    }

    private void OnTileDestroyed(Tile tile)
    {
        if (tile == null || tile.TileData == null) return;

        bool isGoalTile = tile.TileData == goalTileData;

        if (isGoalTile)
        {
            collectedTiles++;
            GameEvents.OnGoalTileCountChanged?.Invoke(gameSettings.GoalTileCount - collectedTiles);

            if (collectedTiles >= gameSettings.GoalTileCount)
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
        if (CurrentState == GameState.GameOver || CurrentState == GameState.GameWon) return;
        CurrentState = newState;
        GameEvents.OnGameStateChanged?.Invoke(newState);
    }
}
