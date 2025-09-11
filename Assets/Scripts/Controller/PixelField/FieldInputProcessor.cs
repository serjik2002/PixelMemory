using System;
using UnityEngine;

public class FieldInputProcessor
{
    private readonly IPixelFieldModel _fieldModel;
    private readonly IInputHandler _inputHandler;

    public event Action OnInputProcessed;

    public FieldInputProcessor(IPixelFieldModel fieldModel, IInputHandler inputHandler)
    {
        _fieldModel = fieldModel ?? throw new ArgumentNullException(nameof(fieldModel));
        _inputHandler = inputHandler ?? throw new ArgumentNullException(nameof(inputHandler));

        SubscribeToInput();
    }

    private void SubscribeToInput()
    {
        _inputHandler.OnPixelPaintRequested += HandlePixelPaintRequest;
        _inputHandler.OnPixelHover += HandlePixelHover;
    }

    public void Dispose()
    {
        _inputHandler.OnPixelPaintRequested -= HandlePixelPaintRequest;
        _inputHandler.OnPixelHover -= HandlePixelHover;
    }

    private void HandlePixelPaintRequest(Vector2Int position, PixelColorType color)
    {
        if (_fieldModel.IsValidPosition(position.x, position.y))
        {
            _fieldModel.SetColor(position.x, position.y, color);
            OnInputProcessed?.Invoke();
        }
        else
        {
            Debug.LogWarning($"Invalid pixel position: {position}");
        }
    }

    private void HandlePixelHover(Vector2Int position)
    {
        // Можно добавить логику подсветки при наведении
        // Пока оставляем пустым
    }

    public void SetInputEnabled(bool enabled)
    {
        _inputHandler?.SetEnabled(enabled);
    }
}
