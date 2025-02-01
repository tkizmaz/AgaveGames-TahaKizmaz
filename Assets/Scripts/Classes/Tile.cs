using UnityEngine;

public class Tile : MovableObject
{
    private TileColor tileColor;
    public TileColor TileColor => tileColor; // Tile'ın rengini dışarıdan erişilebilir yap

    public override void SpawnObject()
    {
        tileColor = (TileColor)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(TileColor)).Length);
        gameObject.GetComponent<SpriteRenderer>().sprite = ColorManager.Instance.GetSprite(tileColor);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }
}
