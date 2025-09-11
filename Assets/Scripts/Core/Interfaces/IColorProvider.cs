using System;

public interface IColorProvider
{
    PixelColorType SelectedColor { get; }
    event Action<PixelColorType> OnColorChanged;
}
