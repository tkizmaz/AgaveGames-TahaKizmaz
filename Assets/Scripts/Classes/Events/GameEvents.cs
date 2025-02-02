using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<int> OnMoveMade; 
    public static Action<TileData> OnGoalTileChanged; 
    public static Action<int> OnGoalTileCountChanged; 
    public static Action<Tile> OnTileDestroyed; 
    public static Action OnGameWon;
    public static Action OnGameLost;
    public static Action<GameState> OnGameStateChanged;
}
