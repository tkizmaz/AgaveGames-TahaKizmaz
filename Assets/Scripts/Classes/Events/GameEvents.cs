using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<int> OnMoveMade; 
    public static Action<TileData> OnTargetTileChanged; 
    public static Action<int> OnTargetTileCountChanged; 
    public static Action<Tile> OnTileDestroyed; 
    public static Action OnGameWon;
    public static Action OnGameLost;
    public static Action<GameState> OnGameStateChanged;
}
