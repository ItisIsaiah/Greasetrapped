using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : NetworkBehaviour
{
   
    public int minigamesTotal=3;
    bool gameStarted;
    float countdownTime=5f;
    public List<GameObject> spawnPoints;
    public NetworkVariable<int> minigamesFinished=new NetworkVariable<int>();
   // public NetworkVariable<List<int>> pID= new NetworkVariable<List<int>>();
    public NetworkList<ulong> playerClientIds;
    public NetworkVariable<int> playersAlive;
    public bool isUI;
    public TextMeshProUGUI textmeshPro;
    

// Initialize in Awake or Start
    void Start()
    {
         playerClientIds = new NetworkList<ulong>();
    }
    public int pCount;
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
        DontDestroyOnLoad(gameObject);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsHost)
        {

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
            pCount = NetworkManager.Singleton.ConnectedClientsList.Count;
            playersAlive.Value = pCount;
        }
    }


      

    

    public List<GameObject> GetPlayerGameObjects()
    {
        List<GameObject> players = new List<GameObject>();

        foreach (var clientId in playerClientIds)
        {
            if (NetworkManager.Singleton.ConnectedClients.TryGetValue(clientId, out var client))
            {
                players.Add(client.PlayerObject.gameObject);
            }
        }

        return players;
    }
    private void OnClientConnected(ulong clientId)
    {
        if (IsHost)
        {
            Debug.Log($"{clientId}");
            playerClientIds.Add(clientId);
            pCount++;
            // Optionally: Broadcast player count to all clients
            UpdatePlayerCountServerRpc();
        }
    }

    private void OnClientDisconnected(ulong clientId)
    {
        if (IsHost)
        {
            playerClientIds.Remove(clientId);
            pCount--;
            // Optionally: Broadcast player count to all clients
            UpdatePlayerCountServerRpc();
        }
    }

    [ServerRpc]
    private void UpdatePlayerCountServerRpc()
    {
        // Send player count to all clients
        PlayerCountClientRpc(pCount);
    }

    [ClientRpc]
    private void PlayerCountClientRpc(int count)
    {
        // Update player count on clients
        pCount = count;
    }



// Update is called once per frame
    void Update()
    {
        if (!IsHost||isUI) return;
        if (minigamesFinished.Value==minigamesTotal )
        {
            Debug.Log("Completed all the tasks");
            //Go to the next scene
        }

        
    }

    public void CompletedMinigame(MinigameManager m)
    {
        m.isCompleted.Value=true;
        minigamesFinished.Value++;
        textmeshPro.text = "TASK FINISHED"+ minigamesFinished.Value+" / "+minigamesTotal;
    }

    public void ResetTask(MinigameManager m)
    {
        m.isCompleted.Value=false;
        minigamesFinished.Value--;
    }

    [ServerRpc]
    public void PlayersDeadServerRpc()
    {
        playersAlive.Value -= 0;
        if (playersAlive.Value <=0)
        {
            NetworkManager.SceneManager.LoadScene("YOU ALL LOSE",LoadSceneMode.Single);
        }
    }
    public IEnumerator StartCountdown()
    {
        playersAlive.Value = playerClientIds.Count;
        textmeshPro = GameObject.FindGameObjectWithTag("TaskUI").GetComponent<TextMeshProUGUI>();
        spawnPoints = GameObject.FindGameObjectsWithTag("Spawnpoint").ToList();
        yield return new WaitForSeconds(countdownTime);
        gameStarted = true;
        int i = 0;
        foreach (GameObject g in GetPlayerGameObjects())
        {
            g.transform.position=spawnPoints[i].transform.position;
            i++;
        }
    }

    public bool HasGameStarted()
    {
        return gameStarted;
    }
}
