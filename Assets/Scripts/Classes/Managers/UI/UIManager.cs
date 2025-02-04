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

    private void OnEnable()
    {
        GameEvents.OnMoveMade += UpdateMovesUI;
        GameEvents.OnScoreChanged += UpdateScoreUI;
        GameEvents.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameEvents.OnMoveMade -= UpdateMovesUI;
        GameEvents.OnScoreChanged -= UpdateScoreUI;
        GameEvents.OnGameStateChanged -= OnGameStateChanged;
    }

    private void UpdateMovesUI(int movesLeft)
    {
        movesValueText.text = movesLeft.ToString();
    }

    private void UpdateScoreUI(int currentScore)
    {
        scoreValueText.text = currentScore.ToString();
    }

    private void OnGameStateChanged(GameState state)
    {
        if (state == GameState.GameOver || state == GameState.GameWon)
        {
            gameOverPanel.SetActive(true);
        }
        string endGameInformation = state == GameState.GameOver ? "Game Over" : "You Win!";
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
