using System;
using System.Diagnostics;
using UnityEngine;

public class PixelFieldModel
{
    private readonly int _rows;
    private readonly int _cols;
    private PixelColorType[,] _pixelsData;

    public int Rows => _rows;
    public int Cols => _cols;

    public event Action<int, int, PixelColorType> OnPixelChanged;
    public event Action OnModelInitialized;

    public PixelFieldModel(int rows, int cols)
    {
        _rows = rows;
        _cols = cols;
        InitializeField();
    }

    public void InitializeField()
    {
        _pixelsData = new PixelColorType[_rows, _cols];
        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                _pixelsData[row, col] = PixelColorType.White;
            }
        }

        OnModelInitialized?.Invoke();
    }

    public void InitializeFromArray(int[,] inputArray)
    {
        if (inputArray.GetLength(0) != _rows || inputArray.GetLength(1) != _cols)
            throw new ArgumentException("Input array size does not match field dimensions.");

        _pixelsData = new PixelColorType[_rows, _cols];

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                int value = inputArray[row, col];
                PixelColorType color = (PixelColorType)value;
                _pixelsData[row, col] = color;
            }
        }

        OnModelInitialized?.Invoke();
    }

    public void SetColor(int row, int col, PixelColorType color)
    {
        if (InBounds(row, col))
        {
            _pixelsData[row, col] = color;
            OnPixelChanged?.Invoke(row, col, color);
            UnityEngine.Debug.Log("SetColor");
        }
    }

    public PixelColorType GetColor(int row, int col)
    {
        if (InBounds(row, col))
            return _pixelsData[row, col];
        return PixelColorType.Black;
    }

    private bool InBounds(int row, int col)
    {
        return row >= 0 && row < _rows && col >= 0 && col < _cols;
    }
}
