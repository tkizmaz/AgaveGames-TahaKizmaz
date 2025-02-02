using UnityEngine;

public static class TileFactory
{
    public static Tile CreateTile(Tile tile, TileData tileData, Transform parent)
    {
        switch (tileData.tileType)
        {
            case TileType.Chip:
                if (!(tile is Chip))
                {
                    tile = tile.gameObject.AddComponent<Chip>();
                }
                break;

            default:
                Debug.LogWarning($"This tile type is not implemented: {tileData.tileType}");
                break;
        }

        tile.SetTileData(tileData);
        tile.transform.SetParent(parent);
        return tile;
    }
}
