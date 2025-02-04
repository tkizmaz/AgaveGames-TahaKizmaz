using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TMP_Text movesValueText; 
    [SerializeField] private TMP_Text targetTileCountText; 
    [SerializeField] private TMP_Text endGameText;
    [SerializeField] private TMP_Text scoreValueText;
    [SerializeField] private GameObject gameOverPanel;
    private int targetScore;
    private int currentScore;

    private void OnEnable()
    {
        GameEvents.OnMoveMade += UpdateMovesUI;
        GameEvents.OnScoreChanged += UpdateScoreUI;
        GameEvents.OnGoalScoreChanged += UpdateGoalScoreUI;
        GameEvents.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnMoveMade -= UpdateMovesUI;
        GameEvents.OnScoreChanged -= UpdateScoreUI;
        GameEvents.OnGoalScoreChanged -= UpdateGoalScoreUI;
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
    }

    private void UpdateScoreUI(int newScore)
    {
        currentScore = newScore;
        UpdateScoreDisplay();
    }

    private void UpdateGoalScoreUI(int newTargetScore)
    {
        targetScore = newTargetScore;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        scoreValueText.text = $"{currentScore} / {targetScore}";
    }

    private void UpdateMovesUI(int movesLeft)
    {
        movesValueText.text = movesLeft.ToString();
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.GameLost || state == GameState.GameWon)
        {
            gameOverPanel.SetActive(true);
        }
        string endGameInformation = state == GameState.GameLost ? "You Lost" : "You Win!";
        endGameText.text = endGameInformation;
    }

    public void ResetUI()
    {
        gameOverPanel.SetActive(false);
    }

    public void OnRestartButtonClicked()
    {
        GameManager.Instance.ResetGame();
    }
}
