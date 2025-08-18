using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickerView : MonoBehaviour
{
    [SerializeField] private ColorPickerModel _model; // ������ �� ������ ��������� ������

    [SerializeField] private Image _colorCircle;   // ��� ������ � ������
    [SerializeField] private Image _selectionRing; // ������ ���������

    [SerializeField] private PixelColorType _colorType;

    public PixelColorType ColorType => _colorType;

    private void Start()
    {
        _model.OnModelInitialized.AddListener(Initialize);
        _model.OnColorChanged.AddListener(UpdateSelectedColor);
    }
    private void Initialize()
    {
        _colorCircle.color = ColorData.ColorMap[_colorType];
        UpdateSelectedColor();

    }

    public void UpdateSelectedColor()
    {
        if (_model.SelectedColor == _colorType)
        {
            _selectionRing.gameObject.SetActive(true);
        }
        else
        {
            _selectionRing.gameObject.SetActive(false);
        }
    }
   
}
