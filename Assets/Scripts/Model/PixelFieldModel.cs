using System;
using System.Diagnostics;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class PixelFieldModel
{
    private int _rows;
    private int _cols;
    private PixelColorType[,] _pixelsData;

    public int Rows => _rows;
    public int Cols => _cols;

    public event Action<PixelData> OnPixelChanged;
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
        _pixelsData = new PixelColorType[_rows, _cols];

        // Заполняем белым цветом по умолчанию
        FillWithColor(PixelColorType.White);

        // Создаем копию для передачи
        //var colorsCopy = new PixelColorType[_rows, _cols];
        //Array.Copy(_pixelsData, colorsCopy, _pixelsData.Length);

        //OnFieldInitialized?.Invoke(new FieldData(_rows, _cols, colorsCopy));
        UnityEngine.Debug.Log($"Модель поля заинитилась");
    }

    public void SetColor(int row, int col, PixelColorType color)
    {
        if (!IsValidPosition(row, col))
        {
            UnityEngine.Debug.LogWarning($"Invalid position: ({row}, {col})");
            return;
        }

        if (_pixelsData[row, col] == color)
            return; // Избегаем лишних уведомлений

        _pixelsData[row, col] = color;
        OnPixelChanged?.Invoke(new PixelData(new Vector2Int(row, col), color));
    }

    public PixelColorType GetColor(int row, int col)
    {
        return IsValidPosition(row, col) ? _pixelsData[row, col] : PixelColorType.Black;
    }

    public void FillWithColor(PixelColorType color)
    {
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                _pixelsData[row, col] = color;
            }
        }
    }

    public void Clear()
    {
        FillWithColor(PixelColorType.White);

        // Уведомляем об очистке всего поля
        //var colorsCopy = new PixelColorType[_rows, _cols];
        //Array.Copy(_pixelsData, colorsCopy, _pixelsData.Length);
        //OnFieldInitialized?.Invoke(new FieldData(_rows, _cols, colorsCopy));
    }

    private bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < _rows && col >= 0 && col < _cols;
    }
}