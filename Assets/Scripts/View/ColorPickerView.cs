using UnityEngine;
using UnityEngine.UI;

public class ColorPickerView : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image _colorCircle;
    [SerializeField] private Image _selectionRing;
    [SerializeField] private Button _button;

    [Header("Settings")]
    [SerializeField] private PixelColorType _colorType;

    private IColorProvider _colorProvider;

    public PixelColorType ColorType => _colorType;

    private void Awake()
    {
    }

    private void Start()
    {
        // Находим провайдер цветов на сцене
        var controller = FindAnyObjectByType<ColorPickerController>();
        if (controller != null)
        {
            Initialize(controller.ColorProvider);
        }
        else
        {
            Debug.LogError("ColorPickerController not found on scene!");
        }
        SetupButton();
    }

    public void Initialize(IColorProvider colorProvider)
    {
        _colorProvider = colorProvider;

        // Подписываемся на изменения
        _colorProvider.OnColorChanged += UpdateSelectionVisual;

        // Устанавливаем цвет круга
        UpdateColorDisplay();

        // Обновляем визуал выделения
        UpdateSelectionVisual(_colorProvider.SelectedColor);

        Debug.Log($"ColorPickerView for {_colorType} initialized");
    }

    private void SetupButton()
    {
        if (_button == null)
            _button = GetComponent<Button>();
        _button.onClick.AddListener(OnButtonClick);

        //if (_button != null)
        //{
            
        //}
        //else
        //{
        //    Debug.LogError($"Button component not found on {gameObject.name}");
        //}
    }

    private void UpdateColorDisplay()
    {
        if (_colorCircle != null && ColorData.ColorMap.ContainsKey(_colorType))
        {
            _colorCircle.color = ColorData.ColorMap[_colorType];
        }
    }

    private void UpdateSelectionVisual(PixelColorType selectedColor)
    {
        bool isSelected = selectedColor == _colorType;

        if (_selectionRing != null)
        {
            _selectionRing.gameObject.SetActive(isSelected);
        }

        // Можно добавить дополнительные эффекты выделения
        if (isSelected)
        {
            transform.localScale = Vector3.one * 1.1f;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    private void OnButtonClick()
    {
        // Находим контроллер и меняем цвет
        var controller = FindAnyObjectByType<ColorPickerController>();
        if (controller != null)
        {
            controller.ChangeColor(_colorType);
        }
        else
        {
            Debug.LogError("ColorPickerController not found!");
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        if (_colorProvider != null)
        {
            _colorProvider.OnColorChanged -= UpdateSelectionVisual;
        }

        if (_button != null)
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }
    }
}