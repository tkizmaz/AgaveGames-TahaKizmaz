using UnityEngine;

public class GameSettings : Singleton<GameSettings>
{
    [Header("Grid Settings")]
    [Range(3, 10)]
    [SerializeField] private int rowCount = 8;
    [Range(3, 10)]
    [SerializeField] private int columnCount = 8;

    [Header("Game Rules Settings")]
    [SerializeField] private int maxMoves = 10;
    [SerializeField] private int goalScore = 100;

    public int RowCount => rowCount;
    public int ColumnCount => columnCount;
    public int MaxMoves => maxMoves;
    public int GoalScore => goalScore;
}
