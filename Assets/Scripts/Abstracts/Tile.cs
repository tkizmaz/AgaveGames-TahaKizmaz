using UnityEngine;

public abstract class Tile : MonoBehaviour, IPoolable
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
        spriteRenderer.sprite = tileData.tileSprite;
    }

    public virtual void SpawnObject()
    {
        TileColor randomColor = (TileColor)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(TileColor)).Length);
        TileData selectedData = TileDatabase.Instance.GetTileDataByColor(randomColor);

        if (selectedData != null)
        {
            SetTileData(selectedData);
        }
        spriteRenderer.sortingOrder = 1;
    }

    public virtual void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnReturnToPool()
    {
        gameObject.SetActive(false);
    }

    public abstract void OnTileDestroyed();
}
