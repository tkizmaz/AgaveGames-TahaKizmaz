public class Chip : MovableTile
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
}
