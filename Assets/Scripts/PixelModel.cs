using System;
using UnityEngine;

public class PixelModel
{
    public Vector2Int Position { get; private set; }
    public PixelColorType Color { get; private set; }

    public event Action<PixelModel> OnColorChanged;

    public PixelModel(Vector2Int position, PixelColorType initialColor = PixelColorType.White)
    {
        Position = position;
        Color = initialColor;
    }

    public void SetColor(PixelColorType newColor)
    {
        if (Color == newColor) return;
        Color = newColor;
        OnColorChanged?.Invoke(this);
    }

}
