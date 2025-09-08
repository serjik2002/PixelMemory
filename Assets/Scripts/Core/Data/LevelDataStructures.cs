using System;
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
    public int[] colors; // �������� � PixelColorType[,] �� int[] ��� ���������� JSON ������������

    // ����� ��� ����������� ����������� ������� � ���������
    public PixelColorType[,] GetColorsAs2D()
    {
        if (colors == null || colors.Length != gridSize.rows * gridSize.cols)
        {
            throw new InvalidOperationException("Colors array size doesn't match grid dimensions");
        }

        var result = new PixelColorType[gridSize.rows, gridSize.cols];
        for (int i = 0; i < colors.Length; i++)
        {
            int row = i / gridSize.cols;
            int col = i % gridSize.cols;
            result[row, col] = (PixelColorType)colors[i];
        }

        return result;
    }
}

public enum PixelColorType
{
    Red = 0,
    Orange = 1,
    LightYellow = 2,
    Green = 3,
    Blue = 4,
    Violet = 5,
    White = 6,
    Black = 7,
    Brown = 8,
    Purple = 9
}

// ��������� ������ ��� �������� ���������� ����� ������
public readonly struct PixelData : IEquatable<PixelData>
{
    public Vector2Int Position { get; }
    public PixelColorType Color { get; }

    public PixelData(Vector2Int position, PixelColorType color)
    {
        Position = position;
        Color = color;
    }

    public bool Equals(PixelData other)
    {
        return Position.Equals(other.Position) && Color == other.Color;
    }

    public override bool Equals(object obj)
    {
        return obj is PixelData other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, (int)Color);
    }
}

public readonly struct FieldData
{
    public int Rows { get; }
    public int Cols { get; }
    public PixelColorType[,] Colors { get; }

    public FieldData(int rows, int cols, PixelColorType[,] colors)
    {
        if (rows <= 0 || cols <= 0)
            throw new ArgumentException("Rows and cols must be positive");

        if (colors?.GetLength(0) != rows || colors?.GetLength(1) != cols)
            throw new ArgumentException("Colors array dimensions don't match provided rows and cols");

        Rows = rows;
        Cols = cols;
        Colors = colors;
    }
}