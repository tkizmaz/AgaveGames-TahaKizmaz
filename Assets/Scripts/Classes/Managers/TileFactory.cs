using UnityEngine;

public static class TileFactory
{
    public static Tile CreateTile(Tile tile, TileData tileData, Transform parent)
    {
        if (tileData is ChipData && !(tile is Chip))
        {
            tile = tile.gameObject.AddComponent<Chip>();
        }

        tile.SetTileData(tileData);
        tile.transform.SetParent(parent);
        return tile;
    }
}
