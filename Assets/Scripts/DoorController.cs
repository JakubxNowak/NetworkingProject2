using Unity.Netcode;
using UnityEngine;

public class DoorController : NetworkBehaviour
{
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer sr;

    // Networked state for the door:
    // false = closed
    // true = open
    // Everyone can read it, but only the server can write it
    public NetworkVariable<bool> IsOpen = new NetworkVariable<bool>(
        false,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    private void Awake()
    {
        // Automatically find components so they do not need to be assigned
        if (col == null) col = GetComponent<Collider2D>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
    }

    public override void OnNetworkSpawn()
    {
        // Apply the current state on spawn (False on start)
        Apply(IsOpen.Value);

        // Apply to clients whenever the server changes the value
        IsOpen.OnValueChanged += (_, newValue) => Apply(newValue);
    }

    private void Apply(bool open)
    {
        // Toggle collision + visibility
        if (col != null) col.enabled = !open;
        if (sr != null) sr.enabled = !open;
    }
}
