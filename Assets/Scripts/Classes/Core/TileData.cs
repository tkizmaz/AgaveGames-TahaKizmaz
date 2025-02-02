using UnityEngine;

[CreateAssetMenu(fileName = "NewTileData", menuName = "Tile System/Tile Data")]
public class TileData : ScriptableObject
{
    public string tileName;
    public TileColor tileColor;
    public TileMoveType moveType;
    public TileSelectableType selectableType;
    public Sprite tileSprite;
}
