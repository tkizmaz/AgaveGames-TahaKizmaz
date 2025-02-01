using UnityEngine;

public class Tile : MovableObject
{
    [SerializeField]
    private TileData tileData;
    public TileData TileData => tileData; // Tile'ın verilerine dışarıdan erişilebilir yap
    private SpriteRenderer spriteRenderer;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetTileData(TileData tileData)
    {
        this.tileData = tileData;
        spriteRenderer.sprite = tileData.tileSprite;
    }

    public override void SpawnObject()
    {
        TileColor randomColor = (TileColor)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(TileColor)).Length);

        TileData selectedData = TileDatabase.Instance.GetTileDataByColor(randomColor);

        if (selectedData != null)
        {
            SetTileData(selectedData);
        }
        spriteRenderer.sortingOrder = 1;
    }
}
