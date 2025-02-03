using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int maxMoves = 10;
    [SerializeField] public int targetCount = 20;
    private int currentMoves;
    private int collectedTiles;
    public GameState CurrentState { get; private set; }
    private TileData goalTileData; 
    private bool isFirstGame = true;

    private void Start()
    {
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
        currentMoves = maxMoves;
        collectedTiles = 0;

        InitializeGoal();

        if(isFirstGame) isFirstGame = false;
        else
        {
            GridManager.Instance.ResetGrid();
        }

        GameEvents.OnMoveMade?.Invoke(currentMoves);
        GameEvents.OnGoalTileCountChanged?.Invoke(targetCount);
        ChangeState(GameState.WaitingForInput);

        UIManager.Instance.ResetUI();
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
        CurrentState = newState;
        GameEvents.OnGameStateChanged?.Invoke(newState);
    }
}
