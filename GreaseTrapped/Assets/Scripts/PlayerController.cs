using Unity.Netcode;
using UnityEngine;
using Cinemachine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : NetworkBehaviour
{
    Vector3 direction;
    public Transform cam;
    float turnSmoothVelocity;

    public Transform pushPoint;
    public float pushRadius = 1f;
    public float pushForce = 5f;

    public float smoothTurn;
    public float speed;
    public GameObject playerVFX;
    public bool miniGameState;
    public CinemachineFreeLook c;
    public Animator animator;
    int currP = 0;
    public NetworkVariable<bool> dead = new NetworkVariable<bool>();
    public NetworkVariable<int> spectatorID = new NetworkVariable<int>(default,
       NetworkVariableReadPermission.Owner, NetworkVariableWritePermission.Owner);
    List<GameObject> players = new List<GameObject>();
    GameObject SpectatorUI;


    void Awake()
    {
        SpectatorUI = GameObject.FindGameObjectWithTag("SpectatorUI");
        SpectatorUI.SetActive(false);
        
        miniGameState = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Initialize()
    {
        spectatorID.Value = Random.Range(0, 9999);
        dead.Value = false;
        if (!IsOwner) return;
        cam = Camera.main.transform;
        c = GameObject.FindGameObjectWithTag("YourCamera").GetComponent<CinemachineFreeLook>();
        c.LookAt = transform;
        c.Follow = transform;
        transform.position= GameManager.Instance.spawnPoints[0].transform.position;
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Initialize();
    }
    private void Update()
    {
        if (!IsOwner || !Application.isFocused || miniGameState) return;

        Debug.Log("I WORK DAD!" + dead.Value);
        if (!dead.Value) {
            Debug.Log("Alive!");
            float Vert = Input.GetAxisRaw("Vertical");
            float Hori = Input.GetAxisRaw("Horizontal");


            direction = new Vector3(Hori, 0f, Vert).normalized;
            animator.SetFloat("speed", direction.magnitude);
            //float runAnim = Mathf.Abs(Hori + Vert);
            if (direction.magnitude >= .1)
            {
                AudioManager.instance.Play("playerbreath");

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTurn);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                transform.position += moveDir.normalized * speed * Time.deltaTime;

            }
            if (Input.GetMouseButtonDown(0))
            {
                //Push if the player is in teh radius 

                Collider[] hits = Physics.OverlapSphere(pushPoint.position, pushRadius);
                foreach (Collider h in hits)
                {
                    if (h.gameObject != this.gameObject && h.CompareTag("player"))
                    {
                        h.GetComponent<Rigidbody>().AddForce(transform.forward * 3f, ForceMode.Impulse);
                        h.GetComponent<PlayerController>().StartCoroutine(Stun(3f));
                    }
                }
                //stun them for x amount of time

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
        if (!IsOwner) return;
        Debug.Log("Player has died");
        // Disable visibility and all control from the player. Add the specate ability. 
        playerVFX.SetActive(false);
        dead.Value = true;
        //if all players are dead, display the deathscreen
        SpectatorUI.SetActive(true);
        players = GameManager.Instance.GetPlayerGameObjects();
        GameManager.Instance.PlayersDeadServerRpc();

    }

    public IEnumerator Stun(float stunTImer)
    {
        
        AudioManager.instance.Play("playerstun");
        yield return new WaitForSeconds(stunTImer);
        AudioManager.instance.Stop("playerstun");
    }

    public void Spectator(int currP)
    {
        if (!IsOwner) return;



             c.LookAt=players[currP].transform;
            c.Follow=players[currP].transform ;
        
    }

    public void instaKill()
    {
        dead.Value = true ;
    }
}
