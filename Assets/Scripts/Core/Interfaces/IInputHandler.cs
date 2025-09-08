using System;
using UnityEngine;

public interface IInputHandler
{
    event Action<Vector2Int, PixelColorType> OnPixelPaintRequested;
    event Action<Vector2Int> OnPixelHover;
    void SetEnabled(bool enabled);
}
