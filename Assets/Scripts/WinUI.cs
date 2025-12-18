using UnityEngine;

public class WinUI : MonoBehaviour
{
    public static WinUI Instance;

    private void Awake()
    {
        // Store a reference to this WinUI in a static field
        Instance = this;

        // Hide the win UI by default when the scene loads.
        gameObject.SetActive(false);
    }

    // Called when the game is won from FinishZone
    public void Show()
    {
        // Enable the whole UI object so it becomes visible
        gameObject.SetActive(true);
    }
}
