using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private int currentMoves;
    private int currentScore;
    public GameState CurrentGameState { get; private set; }
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

    public void ResetGame()
    {
        currentMoves = gameSettings.MaxMoves;
        currentScore = 0;
        if(isFirstGame) isFirstGame = false;
        else
        {
            GridManager.Instance.ResetGrid();
        }

        GameEvents.OnMoveMade?.Invoke(currentMoves);
        GameEvents.OnScoreChanged?.Invoke(currentScore);
        CurrentGameState = GameState.WaitingForInput;
        UIManager.Instance.ResetUI();
    }

    private void OnTileDestroyed(Tile tile)
    {
        if(tile == null) return;
        currentScore += tile.TileData.scoreValue;
        GameEvents.OnScoreChanged?.Invoke(currentScore);
    }


    public void OnMoveMade()
    {
        if (currentMoves <= 0) return; 
        currentMoves--;
        GameEvents.OnMoveMade?.Invoke(currentMoves);

        if (currentMoves == 0)
        {
            CheckGameOver();
        }
    }


    private void CheckGameOver()
    {
        if(currentScore >= gameSettings.GoalScore)
        {
            ChangeState(GameState.GameWon);
        }
        else
        {
            ChangeState(GameState.GameLost);
        }
    }

    public void ChangeState(GameState newState)
    {        
        if (CurrentGameState == GameState.GameLost || CurrentGameState == GameState.GameWon) return;
        CurrentGameState = newState;
        GameEvents.OnGameStateChanged?.Invoke(newState);
    }
}
