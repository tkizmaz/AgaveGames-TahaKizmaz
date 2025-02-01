using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<int> OnMoveMade; 
    public static Action<TileColor> OnTargetTileChanged; 
    public static Action<int> OnTargetTileCountChanged; 
    public static Action<Tile> OnTileDestroyed; 
    public static Action OnGameWon;
    public static Action OnGameLost;
}
