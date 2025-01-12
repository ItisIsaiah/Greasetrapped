using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{

    public MinigameManager[] minigames;
    public NetworkVariable<int> minigamesFinished=new NetworkVariable<int>();
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsServer) return;
        if (minigamesFinished.Value==minigames.Length)
        {
            Debug.Log("Completed all the tasks");
            //Go to the next scene
        }
    }

    public void CompletedMinigame(MinigameManager m)
    {
        m.isCompleted.Value=true;
        minigamesFinished.Value++;
    }

    public void ResetTask(MinigameManager m)
    {
        m.isCompleted.Value=false;
        minigamesFinished.Value--;
    }
}
