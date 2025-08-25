using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class LevelLoader : MonoBehaviour
{
    public string fileName = "level1.json";
    private LevelConfig loadedLevel;

    void Start()
    {
        LoadLevel(1);

        // ������ �������������
        Debug.Log($"�������� �������: {loadedLevel.level}");
        Debug.Log($"������: {loadedLevel.gridSize.rows} x {loadedLevel.gridSize.cols}");
        Debug.Log($"���� � (0,0): {loadedLevel.colors[0][0]}");
        Debug.Log($"���� � (0,0): {loadedLevel.colors[0][0]}");
    }

    public void LoadLevel(int levelIndex)
    {
        // ��������� ���� � Resources ��� ���������� .json
        string fileName = $"Levels/level_{levelIndex}";

        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

        if (jsonFile != null)
        {
            loadedLevel = JsonConvert.DeserializeObject<LevelConfig>(jsonFile.text);
            Debug.Log($"������� {loadedLevel.level} ��������!");
            Debug.Log($"������: {loadedLevel.gridSize.rows} x {loadedLevel.gridSize.cols}");
            Debug.Log($"���� � (0,0): {loadedLevel.colors[0][0]}");
        }
        else
        {
            Debug.LogError("���� �� ������ � Resources: " + fileName);
        }
    }
}
