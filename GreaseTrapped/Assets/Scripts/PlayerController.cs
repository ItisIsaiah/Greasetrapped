using Unity.Netcode;
using UnityEngine;
using Cinemachine;

public class PlayerController : NetworkBehaviour
{
    Vector3 direction;
    public Transform cam;
    float turnSmoothVelocity;
    

    
    public float smoothTurn;
    public float speed;

    public bool miniGameState;
    public CinemachineFreeLook c;
    void Initialize()
    {
        cam=Camera.main.transform;
        c=Transform.FindObjectOfType<CinemachineFreeLook>();
        c.LookAt=transform;
        c.Follow = transform;
        miniGameState = false;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
           Initialize();
    }
    private void Update()
    {
        if (!IsOwner||!Application.isFocused||miniGameState) return;
        float Vert = Input.GetAxisRaw("Vertical");
        float Hori = Input.GetAxisRaw("Horizontal");


        direction = new Vector3(Hori, 0f, Vert).normalized;
        //float runAnim = Mathf.Abs(Hori + Vert);
        if (direction.magnitude >= .1)
        {


            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurn);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.position += moveDir.normalized * speed * Time.deltaTime;

        }
    }
}
