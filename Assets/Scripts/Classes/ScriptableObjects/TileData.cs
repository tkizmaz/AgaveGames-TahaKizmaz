using UnityEngine;

[CreateAssetMenu(fileName = "NewTileData", menuName = "Tile System/Tile Data")]
public class TileData : ScriptableObject
{
    public string tileName;
    public TileMoveType moveType;
    public TileSelectableType selectableType;
    public TileType tileType;
    public Sprite tileSprite;
    public int scoreValue;
}
