using UnityEngine;

public static class TileFactory
{
    public static Tile CreateTile(Tile tile, TileData tileData, Transform parent)
    {
        if (tileData == null || tile == null) return null;

        switch (tileData.tileType)
        {
            case TileType.Chip:
                if (!(tile is Chip))
                {
                    tile = tile.gameObject.AddComponent<Chip>();
                }
                break;

            default:
                Debug.LogWarning($"TileFactory: Unimplemented tile type: {tileData.tileType}");
                return null;
        }

        tile.SetTileData(tileData);
        tile.transform.SetParent(parent);
        return tile;
    }
}
