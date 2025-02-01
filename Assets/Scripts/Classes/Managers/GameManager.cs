using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int maxMoves = 10;
    [SerializeField] public int targetCount = 20;
    private TileColor targetColor;
    private int currentMoves;
    private int collectedTiles;
    public GameState CurrentState { get; private set; }

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
        if (CurrentState == GameState.GameWon || CurrentState == GameState.GameOver)
        {
            return;
        }
        
        if (CurrentState == newState) return;

        CurrentState = newState;
        GameEvents.OnGameStateChanged?.Invoke(newState);
    }
}
