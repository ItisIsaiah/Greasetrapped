using Unity.Netcode;
using UnityEngine;

public class PlayerTask : NetworkBehaviour
{
    GameObject canvas;
    private void OnTriggerEnter(Collider other)
    {
        if (!IsOwner) return;
        canvas.SetActive(true);
        if (Input.GetKeyDown("E"))
        {

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsOwner) return;
        canvas.SetActive(false);
    }
}
