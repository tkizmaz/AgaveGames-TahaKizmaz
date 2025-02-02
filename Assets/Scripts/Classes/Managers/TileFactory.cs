using UnityEngine;

public static class TileFactory
{
    public static Tile CreateTile(TileData tileData, ObjectPool<Tile> tilePool, Transform parent)
    {
        Tile tile = tilePool.GetFromPool();
        if (tileData.tileType == TileType.Chip && !(tile is Chip))
        {
            tile = tile.gameObject.AddComponent<Chip>();
        }

        tile.SetTileData(tileData);
        tile.transform.SetParent(parent);
        return tile;
    }
}
