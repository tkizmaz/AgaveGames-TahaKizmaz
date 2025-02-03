using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TMP_Text movesValueText; 
    [SerializeField] private TMP_Text targetTileCountText; 
    [SerializeField] private TMP_Text endGameText;
    [SerializeField] private Image targetTileImage; 
    [SerializeField] private GameObject gameOverPanel;

    private void OnEnable()
    {
        GameEvents.OnMoveMade += UpdateMovesUI;
        GameEvents.OnGoalTileChanged += SetTargetUI;
        GameEvents.OnGoalTileCountChanged += UpdateTargetTileCount; 
        GameEvents.OnGameStateChanged += OnGameStateChanged;

    }

    private void OnDisable()
    {
        GameEvents.OnMoveMade -= UpdateMovesUI;
        GameEvents.OnGoalTileChanged -= SetTargetUI;
        GameEvents.OnGoalTileCountChanged -= UpdateTargetTileCount;
    }

    private void UpdateMovesUI(int movesLeft)
    {
        movesValueText.text = movesLeft.ToString();
    }

    private void SetTargetUI(TileData targetTileData)
    {
        targetTileImage.sprite = targetTileData.tileSprite;
    }


    private void UpdateTargetTileCount(int remainingTargetCount)
    {
        targetTileCountText.text = remainingTargetCount < 0 ? "0" : remainingTargetCount.ToString();
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

    private void ResetUI()
    {
        gameOverPanel.SetActive(false);
    }

    public void OnRestartButtonClicked()
    {
        GameManager.Instance.ResetGame();
        ResetUI();
    }
}
