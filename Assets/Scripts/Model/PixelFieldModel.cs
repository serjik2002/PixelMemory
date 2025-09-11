using System;
using UnityEngine;

public class PixelFieldModel : IPixelFieldModel
{
    private int _rows;
    private int _cols;
    private PixelModel[,] _pixelsData;

    public int Rows => _rows;
    public int Cols => _cols;

    public event Action<PixelData> OnPixelChanged;
    public event Action OnFieldCleared;

    //public event Action<FieldData> OnFieldInitialized;

    public PixelFieldModel(int rows, int cols)
    {
        Initialize(rows, cols);
    }

    public void Initialize(int rows, int cols)
    {
        if (rows <= 0 || cols <= 0)
            throw new ArgumentException("Rows and cols must be positive");

        _rows = rows;
        _cols = cols;
        _pixelsData = new PixelModel[_rows, _cols];

        // Заполняем белым цветом по умолчанию
        FillWithColor(PixelColorType.White);

        UnityEngine.Debug.Log($"Модель поля заинитилась");
    }

    public void SetColor(int row, int col, PixelColorType color)
    {
        if (!IsValidPosition(row, col))
        {
            UnityEngine.Debug.LogWarning($"Invalid position: ({row}, {col})");
            return;
        }

        if (_pixelsData[row, col].Color == color)
            return; // Избегаем лишних уведомлений

        _pixelsData[row, col].SetColor(color);
        OnPixelChanged?.Invoke(new PixelData(new Vector2Int(row, col), color));
    }

    public PixelColorType GetColor(int row, int col)
    {
        return IsValidPosition(row, col) ? _pixelsData[row, col].Color : PixelColorType.Black;
    }

    public void FillWithColor(PixelColorType color)
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                _pixelsData[row, col] = new PixelModel(new Vector2Int(row, col));
            }
        }
    }

    public void Clear()
    {
        FillWithColor(PixelColorType.White);
    }

    private bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < _rows && col >= 0 && col < _cols;
    }

    bool IPixelFieldModel.IsValidPosition(int row, int col)
    {
        return IsValidPosition(row, col);
    }
    public PixelModel[,] GetPixels()
    {
        return _pixelsData;
    }
}