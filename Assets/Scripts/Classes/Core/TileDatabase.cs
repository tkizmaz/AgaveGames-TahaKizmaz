using UnityEngine;
using System.Collections.Generic;

public class TileDatabase : MonoBehaviour
{
    public static TileDatabase Instance { get; private set; }

    [SerializeField] private List<TileData> allTileDataList; 

    private void Awake()
    {
        Instance = this;
    }

    public TileData GetRandomTileData()
    {
        if (allTileDataList == null || allTileDataList.Count == 0)
        {
            return null;
        }

        return allTileDataList[Random.Range(0, allTileDataList.Count)];
    }
}
