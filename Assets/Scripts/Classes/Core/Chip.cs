using UnityEngine;
public class Chip : MovableTile, ISelectable
{
    private ChipColor chipColor;

    public override void SetTileData(TileData tileData)
    {
        base.SetTileData(tileData);
        
        if (tileData is ChipData chipData)
        {
            chipColor = chipData.chipColor;
            spriteRenderer.sortingOrder = 1;
        }
    }

    public void SetSelected(bool isSelected)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isSelected ? Color.gray : Color.white;
        }
    }

    public override void OnReturnToPool()
    {
        SetSelected(false);
        base.OnReturnToPool();
    }
}
