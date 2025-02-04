using System;
using UnityEngine;

public static class GameEvents
{
    public static Action<int> OnMoveMade; 
    public static Action<int> OnScoreChanged;
    public static Action<int> OnGoalScoreChanged;
    public static Action<Tile> OnTileDestroyed; 
    public static Action<GameState> OnGameStateChanged;
}
