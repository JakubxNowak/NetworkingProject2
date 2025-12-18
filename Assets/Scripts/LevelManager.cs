using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class LevelManager : NetworkBehaviour
{
    [SerializeField] private DoorController door;
    [SerializeField] private int platesRequired = 2;

    [SerializeField] private BridgeController bridge;

    // Which pressure plate controls the bridge
    [SerializeField] private int bridgePlateId = 0;

    // If true: once the bridge is turned on, it stays on forever
    [SerializeField] private bool bridgeStaysOpen = true;

    // Tracks which plateIds are currently pressed
    private readonly HashSet<int> pressedPlateIds = new HashSet<int>();

    // Latch = once opened, never closes again
    private bool doorLatchedOpen = false;
    private bool bridgeLatchedOpen = false;

    // Called by PressurePlate whenever something steps on/off
    public void SetPlatePressedServer(int plateId, bool pressed)
    {
        // Only the server should run this
        if (!IsServer) return;

        // The bridge is controlled by one specific plateId
        if (bridge != null && plateId == bridgePlateId)
        {
            if (bridgeStaysOpen)
            {
                // Turn on once and never turn off again
                if (pressed && !bridgeLatchedOpen)
                {
                    bridgeLatchedOpen = true;
                    bridge.IsEnabled.Value = true;
                }
            }
            else
            {
                // Debug behaviour: on while pressed, off when released
                bridge.IsEnabled.Value = pressed;
            }
        }

        // Door needs certain amount of platesRequired pressed at the same time.
        // Once open, it stays open.
        if (!doorLatchedOpen)
        {
            if (pressed) pressedPlateIds.Add(plateId);
            else pressedPlateIds.Remove(plateId);

            if (pressedPlateIds.Count >= platesRequired)
            {
                doorLatchedOpen = true;

                if (door != null)
                    door.IsOpen.Value = true;
            }
        }
    }
}
