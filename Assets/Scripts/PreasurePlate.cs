using Unity.Netcode;
using UnityEngine;

public class PressurePlate : NetworkBehaviour
{
    // Unique id for plates, can be changed in inspector for each plates
    [SerializeField] private int plateId = 0;

    // Reference to the LevelManager in the scenem assigned in inspecter
    [SerializeField] private LevelManager levelManager;

    // Count how many objects are currently on top of the plate
    private int objectsOnPlate = 0;

    // Only react to Players
    private bool IsValid(Collider2D other)
    {
        return other.CompareTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // only the sever can manage game state
        if (!IsServer) return;

        if (!IsValid(other)) return;

        objectsOnPlate++;

        // When the first player steps on, plate becomes "pressed"
        if (objectsOnPlate == 1)
        {
            levelManager.SetPlatePressedServer(plateId, true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsServer) return;
        if (!IsValid(other)) return;

        objectsOnPlate--;

        // When the player leaves, plate becomes "released"
        if (objectsOnPlate <= 0)
        {
            objectsOnPlate = 0;
            levelManager.SetPlatePressedServer(plateId, false);
        }
    }
}
