using UnityEngine;

public class ColorPickerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PixelColorType _defaultColor = PixelColorType.White;

    private ColorPickerModel _model;

    // �������� ��� ������� � ������ �����
    public IColorProvider ColorProvider => _model;

    private void Awake()
    {
        InitializeModel();
    }

    private void InitializeModel()
    {
        _model = new ColorPickerModel(_defaultColor);

        // ������������� �� ��������� ��� �������������� ������ ���� �����
        _model.OnColorChanged += HandleColorChanged;

        Debug.Log("ColorPickerController initialized");
    }

    private void HandleColorChanged(PixelColorType newColor)
    {
        // ����� ����� �������� �������������� ������:
        // - �������� �������
        // - ���������
        // - ���������� ���������� �����
        Debug.Log($"Color changed in controller: {newColor}");
    }

    // ��������� ����� ��� ��������� ����� (���������� �� UI)
    public void ChangeColor(PixelColorType newColor)
    {
        _model.ChangeSelectedColor(newColor);
    }

    // ����� ��� ������ �� Unity Events (��������� int)
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