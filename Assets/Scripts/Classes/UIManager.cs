using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text movesValueText; 
    [SerializeField] private TMP_Text targetTileCountText; 
    [SerializeField] private Image targetTileImage; 

    private void OnEnable()
    {
        GameEvents.OnMoveMade += UpdateMovesUI;
        GameEvents.OnTargetTileChanged += SetTargetUI;
        GameEvents.OnTargetTileCountChanged += UpdateTargetTileCount; 
    }

    private void OnDisable()
    {
        GameEvents.OnMoveMade -= UpdateMovesUI;
        GameEvents.OnTargetTileChanged -= SetTargetUI;
        GameEvents.OnTargetTileCountChanged -= UpdateTargetTileCount;
    }

    private void UpdateMovesUI(int movesLeft)
    {
        movesValueText.text = movesLeft.ToString();
    }

    private void SetTargetUI(TileColor color)
    {
        TileData targetTileData = TileDatabase.Instance.GetTileDataByColor(color);
        if (targetTileData != null)
        {
            targetTileImage.sprite = targetTileData.tileSprite;
        }
    }


    private void UpdateTargetTileCount(int remainingTargetCount)
    {
        targetTileCountText.text = remainingTargetCount.ToString();
    }

}
