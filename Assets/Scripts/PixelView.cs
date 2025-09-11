using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class PixelView : MonoBehaviour
{
    private Image _image;

    public Vector2Int Position {  get; private set; }

    public event Action<PixelView> OnClicked;
    public event Action<PixelView> OnHovered;


    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void Init(int row, int col, PixelColorType color)
    {
        Position = new Vector2Int(row, col);
        SetColor(color);
    }

    public void Init(Vector2Int position, PixelColorType color)
    {
        Position = position;
        SetColor(color);
    }

    public void SetAlpha(float alpha)
    {
        var color = _image.color;
        color.a = Mathf.Clamp01(alpha);
        _image.color = color;
    }

    public void SetColor(PixelColorType color)
    {
        _image.color = ColorData.ColorMap[color];
    }

    // Вызываются из UI EventTrigger или Button
    public void OnClick() => OnClicked?.Invoke(this);
    public void OnPointerEnter() => OnHovered?.Invoke(this);
}
