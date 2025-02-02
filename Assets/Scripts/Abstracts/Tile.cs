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

    public virtual void SpawnObject()
    {
        TileColor randomColor = (TileColor)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(TileColor)).Length);
        TileData selectedData = TileDatabase.Instance.GetTileDataByColor(randomColor);

        if (selectedData != null)
        {
            SetTileData(selectedData);
        }
        
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = 1;
        }
    }

    public virtual void OnSpawnFromPool()
    {
        gameObject.SetActive(true);
    }

    public virtual void OnReturnToPool()
    {
        SetSelected(false);
        gameObject.SetActive(false);
    }
    
    public void SetSelected(bool isSelected)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = isSelected ? Color.gray : Color.white;
        }
    }
}