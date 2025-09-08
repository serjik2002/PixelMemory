using System;
using UnityEngine;

public class InputHandler : MonoBehaviour, IInputHandler
{
    [Header("Dependencies")]
    [SerializeField] private IRaycastService _raycastService;
    [SerializeField] private IColorProvider _colorProvider;

    [Header("Settings")]
    [SerializeField] private bool _isEnabled = true;

    public event Action<Vector2Int, PixelColorType> OnPixelPaintRequested;
    public event Action<Vector2Int> OnPixelHover;

    private bool _isDragging;
    private Vector2Int _lastPaintedPixel = new Vector2Int(-1, -1);

    private void Awake()
    {
        // Получаем зависимости если они не заданы
        if (_raycastService == null)
            _raycastService = FindObjectOfType<RaycastService>();

        if (_colorProvider == null)
            _colorProvider = FindObjectOfType<ColorPickerModel>();
    }

    private void Update()
    {
        if (!_isEnabled) return;

        HandleMouseInput();
        HandleHoverInput();
    }

    private void HandleMouseInput()
    {
        bool isMouseDown = Input.GetMouseButtonDown(0);
        bool isMouseHeld = Input.GetMouseButton(0);
        bool isMouseUp = Input.GetMouseButtonUp(0);

        if (isMouseDown)
        {
            _isDragging = true;
            _lastPaintedPixel = new Vector2Int(-1, -1);
            TryPaintPixel();
        }
        else if (isMouseHeld && _isDragging)
        {
            TryPaintPixel();
        }
        else if (isMouseUp)
        {
            _isDragging = false;
            _lastPaintedPixel = new Vector2Int(-1, -1);
        }
    }

    private void HandleHoverInput()
    {
        if (_isDragging) return; // Не отправляем hover события во время рисования

        if (_raycastService.TryGetComponentUnderMouse<Pixel>(out var pixel))
        {
            OnPixelHover?.Invoke(pixel.Position);
        }
    }

    private void TryPaintPixel()
    {
        if (_raycastService.TryGetComponentUnderMouse<Pixel>(out var pixel))
        {
            var position = pixel.Position;

            // Избегаем повторного закрашивания того же пикселя при перетаскивании
            if (_isDragging && position == _lastPaintedPixel)
                return;

            _lastPaintedPixel = position;
            OnPixelPaintRequested?.Invoke(position, _colorProvider.SelectedColor);
        }
    }

    public void SetEnabled(bool enabled)
    {
        _isEnabled = enabled;

        if (!enabled)
        {
            _isDragging = false;
            _lastPaintedPixel = new Vector2Int(-1, -1);
        }
    }

    // Дополнительные методы для настройки поведения
    public void EnableDragPainting(bool enable)
    {
        // Можно добавить отдельную настройку для рисования перетаскиванием
    }

    private void OnDestroy()
    {
        // Очищаем события при уничтожении
        OnPixelPaintRequested = null;
        OnPixelHover = null;
    }
}