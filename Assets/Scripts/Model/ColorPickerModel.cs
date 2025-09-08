using System;
using UnityEngine;
using UnityEngine.Events;

public class ColorPickerModel : MonoBehaviour, IColorProvider
{
    [SerializeField] private PixelColorType _defaultColor;

    private PixelColorType _selectedColor;

    public PixelColorType SelectedColor => _selectedColor;

    public UnityEvent OnColorChanged;
    public UnityEvent OnModelInitialized;

    event Action<PixelColorType> IColorProvider.OnColorChanged
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

    private void Start()
    {
        ChangeSelectedColor(_defaultColor);
        OnModelInitialized.Invoke();
    }

    public void ChangeSelectedColor(PixelColorType newColor)
    {
        if (_selectedColor != newColor)
        {
            _selectedColor = newColor;
            OnColorChanged.Invoke();
        }
        Debug.Log(_selectedColor);
    }
}
