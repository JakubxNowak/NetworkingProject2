using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float smoothSpeed = 8f;

    private Transform target;

    private void LateUpdate()
    {
        // When the player joins as host their player object is not assigned to the camera so we need to try to grab the local player's transform
        if (target == null)
        {
            TryFindLocalPlayer();
            return;
        }

        Vector3 desired = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desired, smoothSpeed * Time.deltaTime);
    }

    private void TryFindLocalPlayer()
    {
        // LocalPlayerObject is the player this client owns (host owns host player, client owns client player)
        if (NetworkManager.Singleton == null) return;
        if (NetworkManager.Singleton.LocalClient == null) return;

        var playerObj = NetworkManager.Singleton.LocalClient.PlayerObject;
        if (playerObj != null)
            target = playerObj.transform;
    }
}
