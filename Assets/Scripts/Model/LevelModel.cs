using System;
using UnityEngine;

public class LevelModel
{
    public int Id { get; private set; }
    public int Rows { get; private set; }
    public int Cols { get; private set; }
    public PixelColorType[,] SolutionPixels { get; private set; }

    // Дополнительные свойства для расширения функциональности
    public string Name { get; private set; }
    public int Difficulty { get; private set; }
    public bool IsCompleted { get; private set; }

    public LevelModel(int id, int rows, int cols, PixelColorType[,] solutionPixels, string name = null, int difficulty = 1)
    {
        if (id < 0) throw new ArgumentException("Level ID must be non-negative", nameof(id));
        if (rows <= 0) throw new ArgumentException("Rows must be positive", nameof(rows));
        if (cols <= 0) throw new ArgumentException("Cols must be positive", nameof(cols));
        if (solutionPixels == null) throw new ArgumentNullException(nameof(solutionPixels));
        if (solutionPixels.GetLength(0) != rows || solutionPixels.GetLength(1) != cols)
            throw new ArgumentException("Solution pixels dimensions don't match provided rows and cols");

        Id = id;
        Rows = rows;
        Cols = cols;
        SolutionPixels = (PixelColorType[,])solutionPixels.Clone(); // Создаем копию для безопасности
        Name = name ?? $"Level {id}";
        Difficulty = Mathf.Clamp(difficulty, 1, 10);
        IsCompleted = false;
    }

    public LevelModel(LevelConfig config) : this(
        config.levelId,
        config.gridSize.rows,
        config.gridSize.cols,
        config.colors)
    {
        // Можно добавить дополнительные поля из config если они есть
    }

    public PixelColorType GetSolutionColor(int row, int col)
    {
        if (!IsValidPosition(row, col))
        {
            Debug.LogWarning($"Invalid position requested: ({row}, {col}) for level {Id}");
            return PixelColorType.White;
        }

        return SolutionPixels[row, col];
    }

    public bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < Rows && col >= 0 && col < Cols;
    }

    public void SetCompleted(bool completed)
    {
        IsCompleted = completed;
    }

    // Метод для проверки правильности решения
    public bool CheckSolution(PixelColorType[,] playerField)
    {
        if (playerField == null) return false;
        if (playerField.GetLength(0) != Rows || playerField.GetLength(1) != Cols) return false;

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                if (playerField[row, col] != SolutionPixels[row, col])
                    return false;
            }
        }

        return true;
    }

    // Подсчет процента выполнения
    public float GetCompletionPercentage(PixelColorType[,] playerField)
    {
        if (playerField == null) return 0f;
        if (playerField.GetLength(0) != Rows || playerField.GetLength(1) != Cols) return 0f;

        int correctPixels = 0;
        int totalPixels = Rows * Cols;

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Cols; col++)
            {
                if (playerField[row, col] == SolutionPixels[row, col])
                    correctPixels++;
            }
        }

        return totalPixels > 0 ? (float)correctPixels / totalPixels : 0f;
    }

    public override string ToString()
    {
        return $"Level {Id}: {Name} ({Rows}x{Cols}), Difficulty: {Difficulty}, Completed: {IsCompleted}";
    }

    public override bool Equals(object obj)
    {
        return obj is LevelModel other && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}