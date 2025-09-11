using System;
using UnityEngine;

public class LevelCompletionChecker
{
    public event Action<float> OnLevelCompleted;

    public bool CheckCompletion(IPixelFieldModel fieldModel, LevelModel levelModel)
    {
        if (fieldModel == null || levelModel == null) return false;

        var playerField = FieldInitializer.CreateFieldDataFromModel(fieldModel).Colors;
        return levelModel.CheckSolution(playerField);
    }

    public float GetCompletionPercentage(IPixelFieldModel fieldModel, LevelModel levelModel)
    {
        if (fieldModel == null || levelModel == null) return 0f;

        var playerField = FieldInitializer.CreateFieldDataFromModel(fieldModel).Colors;
        return levelModel.GetCompletionPercentage(playerField);
    }

    public void ProcessCompletionCheck(IPixelFieldModel fieldModel, LevelModel levelModel)
    {
        if (CheckCompletion(fieldModel, levelModel))
        {
            var completionTime = Time.time; // Можно улучшить с помощью отдельного таймера
            OnLevelCompleted?.Invoke(completionTime);
            Debug.Log("Level completed!");
        }
    }
}
