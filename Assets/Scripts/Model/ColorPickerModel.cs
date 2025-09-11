using System;

public class ColorPickerModel : IColorProvider
{
    private PixelColorType _selectedColor;

    public PixelColorType SelectedColor => _selectedColor;

    // Правильная реализация события
    public event Action<PixelColorType> OnColorChanged;

    public ColorPickerModel(PixelColorType defaultColor = PixelColorType.White)
    {
        _selectedColor = defaultColor;
    }

    public void ChangeSelectedColor(PixelColorType newColor)
    {
        if (_selectedColor != newColor)
        {
            _selectedColor = newColor;
            OnColorChanged?.Invoke(newColor);
            UnityEngine.Debug.Log($"Selected color changed to: {newColor}");
        }
    }
}