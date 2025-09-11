using UnityEngine;

public class ColorPickerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PixelColorType _defaultColor = PixelColorType.White;

    private ColorPickerModel _model;

    // Свойство для доступа к модели извне
    public IColorProvider ColorProvider => _model;

    private void Awake()
    {
        InitializeModel();
    }

    private void InitializeModel()
    {
        _model = new ColorPickerModel(_defaultColor);

        // Подписываемся на изменения для дополнительной логики если нужно
        _model.OnColorChanged += HandleColorChanged;

        Debug.Log("ColorPickerController initialized");
    }

    private void HandleColorChanged(PixelColorType newColor)
    {
        // Здесь можно добавить дополнительную логику:
        // - звуковые эффекты
        // - аналитику
        // - сохранение выбранного цвета
        Debug.Log($"Color changed in controller: {newColor}");
    }

    // Публичный метод для изменения цвета (вызывается из UI)
    public void ChangeColor(PixelColorType newColor)
    {
        _model.ChangeSelectedColor(newColor);
    }

    // Метод для вызова из Unity Events (принимает int)
    public void ChangeColor(int colorIndex)
    {
        if (System.Enum.IsDefined(typeof(PixelColorType), colorIndex))
        {
            _model.ChangeSelectedColor((PixelColorType)colorIndex);
        }
        else
        {
            Debug.LogError($"Invalid color index: {colorIndex}");
        }
    }

    private void OnDestroy()
    {
        if (_model != null)
        {
            _model.OnColorChanged -= HandleColorChanged;
        }
    }
}