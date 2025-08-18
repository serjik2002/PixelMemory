using UnityEngine;
using UnityEngine.Events;

public class ColorPickerModel : MonoBehaviour
{
    [SerializeField] private PixelColorType _defaultColor;

    private PixelColorType _selectedColor;

    public PixelColorType SelectedColor => _selectedColor;

    public UnityEvent OnColorChanged;
    public UnityEvent OnModelInitialized;

    private void Start()
    {
        ChangeColor(_defaultColor);
        OnModelInitialized.Invoke();
    }

    public void ChangeColor(PixelColorType newColor)
    {
        if (_selectedColor != newColor)
        {
            _selectedColor = newColor;
            OnColorChanged.Invoke();
        }
    }
}
