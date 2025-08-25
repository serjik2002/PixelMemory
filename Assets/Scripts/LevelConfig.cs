using UnityEngine;

[System.Serializable]
public class GridSize
{
    public int rows;
    public int cols;
}

[System.Serializable]
public class LevelConfig
{
    public int level;
    public GridSize gridSize;
    public int[][] colors;
}