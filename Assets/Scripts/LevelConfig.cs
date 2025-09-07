using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelConfigWrapper
{
    public List<LevelConfig> levels;
}

[System.Serializable]
public class GridSize
{
    public int rows;
    public int cols;
}

[System.Serializable]
public class LevelConfig
{
    public int levelId;
    public GridSize gridSize;
    public PixelColorType[,] colors;
}