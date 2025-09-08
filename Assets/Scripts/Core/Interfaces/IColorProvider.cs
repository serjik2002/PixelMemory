using System;
using UnityEngine;

public interface IColorProvider
{
    PixelColorType SelectedColor { get; }
    event Action<PixelColorType> OnColorChanged;
}
