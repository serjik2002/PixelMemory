using UnityEngine;
using Newtonsoft.Json;

public class LevelLoader : MonoBehaviour
{
    private LevelConfig loadedLevel;

    void Start()
    {
        LoadLevel(1);

        // Пример использования
        Debug.Log($"Загружен уровень: {loadedLevel.level}");
        Debug.Log($"Размер: {loadedLevel.gridSize.rows} x {loadedLevel.gridSize.cols}");
    }

    public void LoadLevel(int levelIndex)
    {
        // Формируем путь к Resources без расширения .json
        string fileName = $"Levels/level_{levelIndex}";

        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
        

        if (jsonFile != null)
        {
            loadedLevel = JsonConvert.DeserializeObject<LevelConfig>(jsonFile.text);
            Debug.Log($"Уровень {loadedLevel.level} загружен!");
            Debug.Log($"Размер: {loadedLevel.gridSize.rows} x {loadedLevel.gridSize.cols}");
            Debug.Log($"Цвет в (0,0): {loadedLevel.colors[0][0]}");
        }
        else
        {
            Debug.LogError("Файл не найден в Resources: " + fileName);
        }
    }
}

//TODO: