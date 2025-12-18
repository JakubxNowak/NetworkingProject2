using Unity.Netcode;
using UnityEngine;

public class BridgeController : NetworkBehaviour
{
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer sr;

    // Networked state for the bridge:
    // false = bridge is off
    // true = bridge is on
    // Everyone can read it, but only the server can write it
    public NetworkVariable<bool> IsEnabled = new NetworkVariable<bool>(
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
        Apply(IsEnabled.Value);

        // Apply to clients whenever the server changes the value
        IsEnabled.OnValueChanged += (_, newValue) => Apply(newValue);
    }

    private void Apply(bool enabled)
    {
        // Toggle collision/visibility
        if (col != null) col.enabled = enabled;
        if (sr != null) sr.enabled = enabled;
    }
}
