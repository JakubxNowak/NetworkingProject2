using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FinishZone : NetworkBehaviour
{
    // Name of the scene that will be loaded
    [SerializeField] private string winSceneName = "WinScene";

    // Flag to ensure the win logic only triggers once
    private bool triggered = false;

    // Called automatically by Unity when something enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only the server can control game state changes
        if (!IsServer) return;

        // Ignore anything that is not a player
        if (!other.CompareTag("Player")) return;

        // If the win condition already triggered, do nothing
        if (triggered) return;

        // Mark as triggered
        triggered = true;

        // Load the win scene for all players
        LoadWinScene();
    }

    // Loads the win scene using Netcode's SceneManager
    private void LoadWinScene()
    {
        NetworkManager.SceneManager.LoadScene(
            winSceneName,
            LoadSceneMode.Single
        );
    }
}
