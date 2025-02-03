using UnityEngine;
using System.Collections.Generic;

public class TileDatabase : Singleton<TileDatabase>
{
    [SerializeField] private List<TileData> allTileDataList;
    public TileData GetRandomTileData()
    {
        if (allTileDataList == null || allTileDataList.Count == 0)
        {
            return null;
        }
        
        return allTileDataList[Random.Range(0, allTileDataList.Count)];
    }
}
