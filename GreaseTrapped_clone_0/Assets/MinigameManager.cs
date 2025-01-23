using Unity.Netcode;
using UnityEngine;

public class MinigameManager : NetworkBehaviour
{
    public GameObject minigame;
    public Collider C;
    public NetworkVariable<bool> isCompleted=new NetworkVariable<bool>();
    bool isPlaying;
    Minigame currMinigame;
    public string description;


    public void Start()
    {
        Debug.Log("Doing it");
        //minigame = GameObject.FindGameObjectWithTag(tag);
        currMinigame = minigame.GetComponentInChildren<Minigame>();
        
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        currMinigame = minigame.GetComponentInChildren<Minigame>();
        minigame.SetActive(false);
    }

    public void HandleTrigger(Collider other)
    {
        Debug.Log("Somethings here");
        if (!IsOwner) return;
        /*if (minigame == null)
        {
            currMinigame = minigame.GetComponentInChildren<Minigame>();
            minigame.SetActive(false);
            isPlaying = false;
        }
        */
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Felt that E");
                if (!minigame.activeSelf)
                {
                    minigame.SetActive(true);

                    currMinigame.SetUp();

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    isPlaying = true;
                }


            }
            //
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                minigame.SetActive(false);

                currMinigame.Abort();

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isPlaying = false;
            }



        }



    }

    public void CompletedTask()
    {
        isPlaying = false;

        GameManager.Instance.CompletedMinigame(this);
        minigame.SetActive(false);

    }

    public void ResetTask()
    {
        GameManager.Instance.ResetTask(this);
        currMinigame.SetUp();
    }

    public void Update()
    {
        if (!IsOwner||!isPlaying) return;
        currMinigame.GameLoop(this);
    }

    public void HandleLeaving(Collider other)
    {
        Debug.Log("Leaving!");
        if (!IsOwner) return;
        minigame.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
