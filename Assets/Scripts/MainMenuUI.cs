using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeMenuUI : MonoBehaviour
{
    [SerializeField] private string gameSceneName = "GameScene";

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        LoadGameScene();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        // The server is allowed to change scenes in Netcode
        if (NetworkManager.Singleton.IsServer)
        {
            // Use Netcode SceneManager so all clients get the scene switch
            NetworkManager.Singleton.SceneManager.LoadScene(
                gameSceneName,
                LoadSceneMode.Single
            );
        }
    }
}
