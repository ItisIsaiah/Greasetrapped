using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;

public class PlayerController : NetworkBehaviour
{
    Vector3 direction;
    public Transform cam;
    float turnSmoothVelocity;
    

    
    public float smoothTurn;
    public float speed;
    public GameObject playerVFX;
    public bool miniGameState;
    public CinemachineFreeLook c;
    int currP = 0;
    public NetworkVariable<bool> dead=new NetworkVariable<bool>();
    public NetworkVariable<int> spectatorID = new NetworkVariable<int>(default,
       NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
    List<GameObject> players= new List<GameObject>();
    GameObject SpectatorUI;


    void Start()
    {
        SpectatorUI = GameObject.FindGameObjectWithTag("SpectatorUI");
        SpectatorUI.SetActive(false);
    }
    void Initialize()
    {
        cam=Camera.main.transform;
        spectatorID.Value = Random.Range(0,9999);
        dead.Value=false;
        c=GameObject.FindGameObjectWithTag("YourCamera").GetComponent<CinemachineFreeLook>();
        c.LookAt=transform;
        c.Follow = transform;
        miniGameState = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
           Initialize();
    }
    private void Update()
    {
        if (!IsOwner||!Application.isFocused||miniGameState) return;

        Debug.Log(dead.Value);
        if (!dead.Value) {
            Debug.Log("Alive!");
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

        else
        {
            if (Input.GetKeyDown("e"))
            {
                if (currP != players.Count - 1)
                {
                    Spectator(currP + 1);
                }
                else
                {
                    currP = 0;
                    Spectator(currP);
                }
            }
            else if (Input.GetKeyDown("q"))
            {
                if (currP != 0)
                {
                    Spectator(currP - 1);
                }
                else
                {
                    currP = players.Count - 1;
                    Spectator(currP);
                }
            }
        }
    }

    public void Die()
    {
        // Disable visibility and all control from the player. Add the specate ability. 
        playerVFX.SetActive(false);
        dead.Value = true;
        //if all players are dead, display the deathscreen
        SpectatorUI.SetActive(true);
        players = GameManager.Instance.GetPlayerGameObjects();

    }

    public void Spectator(int currP)
    {
       


        
            c.LookAt=players[currP].transform;
            c.Follow=players[currP].transform ;
        
    }

    public void instaKill()
    {
        dead.Value = true ;
    }
}
