using System;
using UnityEngine;

public class Tile : MovableObject
{
    [SerializeField]
    private TileData tileData;
    public TileData TileData => tileData;
    private SpriteRenderer spriteRenderer;

    private void Awake() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void SpawnObject(bool shouldMoveOnSpawn = false, Cell targetCell = null, Action OnComplete = null)
    {
        TileColor randomColor = (TileColor)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(TileColor)).Length);
        TileData selectedData = TileDatabase.Instance.GetTileDataByColor(randomColor);

        if (selectedData != null)
        {
            tileData = selectedData;
            spriteRenderer.sprite = tileData.tileSprite;
        }

        spriteRenderer.sortingOrder = 1;

        if (shouldMoveOnSpawn && targetCell != null)
        {
            MoveToCell(targetCell, OnComplete);
        }
    }

    public override void OnSpawnFromPool()
    {
        SpawnObject();
    }
}
