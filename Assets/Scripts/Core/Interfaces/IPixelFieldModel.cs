using System;
using UnityEngine;

public interface IPixelFieldModel
{
    int Rows { get; }
    int Cols { get; }
    event Action<PixelData> OnPixelChanged;
    event Action OnFieldCleared;

    void SetColor(int row, int col, PixelColorType color);
    PixelColorType GetColor(int row, int col);
    void Clear();
    bool IsValidPosition(int row, int col);
}
