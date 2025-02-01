using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TileDatabase : MonoBehaviour
{
    public static TileDatabase Instance;
    private void Awake() => Instance = this;

    [SerializeField] private List<TileData> tileList;

    public TileData GetTileDataByColor(TileColor color)
    {
        return tileList.FirstOrDefault(tile => tile.tileColor == color);
    }

    public TileData GetRandomTileData()
    {
        return tileList[Random.Range(0, tileList.Count)];
    }
}
