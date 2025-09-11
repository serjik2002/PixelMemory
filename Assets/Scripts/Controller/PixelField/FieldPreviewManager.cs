using System;
using UnityEngine;

public class FieldPreviewManager
{
    private readonly IPixelFieldView _fieldView;
    private MonoBehaviour _coroutineRunner;

    public event Action OnPreviewComplete;

    public FieldPreviewManager(IPixelFieldView fieldView, MonoBehaviour coroutineRunner)
    {
        _fieldView = fieldView ?? throw new ArgumentNullException(nameof(fieldView));
        _coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
    }

    public void ShowLevelPreview(LevelModel levelModel, float duration)
    {
        if (levelModel == null) return;

        var solutionData = new FieldData(
            levelModel.Rows,
            levelModel.Cols,
            levelModel.SolutionPixels
        );

        _fieldView.ShowFieldTemporarily(solutionData, duration, OnPreviewComplete);
    }

    public void ShowCustomPreview(FieldData fieldData, float duration, Action onComplete = null)
    {
        _fieldView.ShowFieldTemporarily(fieldData, duration, () =>
        {
            OnPreviewComplete?.Invoke();
            onComplete?.Invoke();
        });
    }
}
