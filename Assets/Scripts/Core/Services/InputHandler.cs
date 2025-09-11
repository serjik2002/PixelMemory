using System;
using UnityEngine;

public class InputHandler : MonoBehaviour, IInputHandler
{
    [Header("Dependencies")]
    [SerializeField] private IRaycastService _raycastService;
    [SerializeField] private ColorPickerController _colorPickerController;

    [Header("Settings")]
    [SerializeField] private bool _isEnabled = true;

    public event Action<Vector2Int, PixelColorType> OnPixelPaintRequested;
    public event Action<Vector2Int> OnPixelHover;

    private IColorProvider _colorProvider;
    private bool _isDragging;
    private Vector2Int _lastPaintedPixel = new Vector2Int(-1, -1);

    private void Awake()
    {
        InitializeDependencies();
    }

    private void InitializeDependencies()
    {
        // Ќаходим зависимости если не заданы
        if (_raycastService == null)
            _raycastService = FindAnyObjectByType<RaycastService>();

        if (_colorPickerController == null)
            _colorPickerController = FindAnyObjectByType<ColorPickerController>();

        if (_colorPickerController != null)
            _colorProvider = _colorPickerController.ColorProvider;
    }

    private void Update()
    {
        if (!_isEnabled) return;

        HandlePointerInput();
    }

    private void HandlePointerInput()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        bool isDown = Input.GetMouseButtonDown(0);
        bool isHeld = Input.GetMouseButton(0);
        bool isUp = Input.GetMouseButtonUp(0);
        Vector2 position = Input.mousePosition;
#else
        if (Input.touchCount == 0) return;

        var touch = Input.GetTouch(0);
        bool isDown = touch.phase == TouchPhase.Began;
        bool isHeld = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
        bool isUp = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
        Vector2 position = touch.position;
#endif

        if (isDown)
        {
            _isDragging = true;
            _lastPaintedPixel = new Vector2Int(-1, -1);
            HandlePointerDown(position);
        }
        else if (isHeld && _isDragging)
        {
            TryPaintPixel(position);
        }
        else if (isUp)
        {
            _isDragging = false;
            _lastPaintedPixel = new Vector2Int(-1, -1);
        }
    }

    private void HandlePointerDown(Vector2 pointerPosition)
    {
        // —начала провер€ем, не кликнули ли по color picker'у
        if (TrySelectColor(pointerPosition))
            return;

        // ≈сли не по color picker'у, то пытаемс€ покрасить пиксель
        TryPaintPixel(pointerPosition);
    }

    private bool TrySelectColor(Vector2 pointerPosition)
    {
        if (_raycastService.TryGetComponentUnderPointer<ColorPickerView>(pointerPosition, out var colorView))
        {
            _colorPickerController?.ChangeColor(colorView.ColorType);
            return true;
        }
        return false;
    }

    private void TryPaintPixel(Vector2 pointerPosition)
    {
        if (_raycastService.TryGetComponentUnderPointer<PixelView>(pointerPosition, out var pixel))
        {
            var position = pixel.Position;

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

    private void OnDestroy()
    {
        OnPixelPaintRequested = null;
        OnPixelHover = null;
    }
}