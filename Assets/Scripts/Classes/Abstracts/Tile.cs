using UnityEngine;

public class Tile : MonoBehaviour, IPoolable
{
    [SerializeField] protected TileData tileData;
    public TileData TileData => tileData;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public virtual void SetTileData(TileData tileData)
    {
        if (tileData == null) return; 
        
        this.tileData = tileData;
        spriteRenderer.sprite = tileData.tileSprite;
        spriteRenderer.sortingOrder = 1;
    }

    public virtual void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnReturnToPool()
    {
        gameObject.SetActive(false);
        tileData = null;
        spriteRenderer.sprite = null;
    }
}
