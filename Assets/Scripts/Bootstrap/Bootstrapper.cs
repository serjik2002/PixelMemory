using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private string _nextSceneName = "Core";

    [Header("Services")]
    [SerializeField] private LevelLoadServices _levelLoadServices;

    async void Start()
    {
        await InitializeServices();

        SceneManager.LoadScene(_nextSceneName);
    }

    private async Task InitializeServices()
    {
        await _levelLoadServices.Initialize();
        await Task.Delay(500); // Simulate some async initialization work
    }
}
