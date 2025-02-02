using UnityEngine;

public class Tile : MonoBehaviour, IPoolable
{
    [SerializeField]
    protected TileData tileData;
    public TileData TileData => tileData;

    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void SetTileData(TileData tileData)
    {
        this.tileData = tileData;
        if (spriteRenderer != null && tileData != null)
        {
            spriteRenderer.sprite = tileData.tileSprite;
        }
    }

    public virtual void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
