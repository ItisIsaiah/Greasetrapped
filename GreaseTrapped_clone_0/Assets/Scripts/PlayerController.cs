using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{


    private void Update()
    {
        float Vert = Input.GetAxisRaw("Vertical");
        float Horizontal = Input.GetAxisRaw("Horizontal");
    }
}
