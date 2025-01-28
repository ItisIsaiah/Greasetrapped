
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Relay.Models;


public class NetworkSceneManager : NetworkBehaviour
{

    private string ipAddress = "127.0.0.1";
    
    private UnityTransport transport;
    public TMP_InputField inputField;
    public Button readyUpButton;
    NetworkVariable<int> readyPlayers = new NetworkVariable<int>(0,  // default value
    NetworkVariableReadPermission.Everyone,  // anyone can read
    NetworkVariableWritePermission.Server    // only server can write
    );
    public bool isReady=false;
    public TextMeshProUGUI LobbyCount;
    public GameObject StartButton;


    void Start()
    {
        isReady = false;
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        readyPlayers.Value = 0;
        StartButton.SetActive( true );
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(NetworkManager.Singleton.gameObject );

    }





    void UpdateUI(ulong clientId)
    {
        Debug.Log("I ran 1");
        LobbyCount.text = "Ready: " + readyPlayers.Value + "//" + GameManager.Instance.playerClientIds.Count;
    }

    [ClientRpc]
    private void UpdateReadyCountClientRpc()
    {
        Debug.Log("I ran 2");
        LobbyCount.text = "Ready: " + readyPlayers.Value + "//" + GameManager.Instance.playerClientIds.Count; 
        if(IsHost && readyPlayers.Value >= GameManager.Instance.playerClientIds.Count)
        {
            StartButton.SetActive(true);

            Button s = StartButton.GetComponent<Button>();


            if (!IsHost)
            {
                ChangeButtonColor(s,Color.gray);
            }
        }
    }






#if UNITY_EDITOR
    public UnityEditor.SceneAsset SceneAsset;
    private void OnValidate()
    {
        if (SceneAsset != null)
        {
            m_SceneName = SceneAsset.name;
        }
    }
#endif
    [SerializeField]
    private string m_SceneName;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsServer && !string.IsNullOrEmpty(m_SceneName))
        {
            var status = NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Additive);
            if (status != SceneEventProgressStatus.Started)
            {
                Debug.LogWarning($"Failed to load {m_SceneName} " +
                      $"with a {nameof(SceneEventProgressStatus)}: {status}");
            }
        }



        
        
            NetworkManager.Singleton.OnClientConnectedCallback += UpdateUI;
        
    }

    public void JoinButton()
    {
        ipAddress = inputField.text;
        transport.ConnectionData.Address = ipAddress;

        NetworkManager.Singleton.StartClient();
    }

    public void HostButton()
    {
        NetworkManager.Singleton.StartHost();
    }


    public void ReadyUp()
    {
        if (!isReady)
        {
            ChangeButtonColor(readyUpButton, Color.green);
            readyUpButton.GetComponentInChildren<TMP_Text>().text = "Ready";
            AddReadyPlayerServerRpc();
            isReady = true;
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void AddReadyPlayerServerRpc()
    {
        // If you're the server, directly add to ready players
        readyPlayers.Value++;
        UpdateReadyCountClientRpc();
    }

    void ChangeButtonColor(Button b,Color newColor)
    {
        b.colors = new ColorBlock
        {
            normalColor = newColor,
            highlightedColor = newColor * 1.2f,  // Slightly brighter on hover
            pressedColor = newColor * 0.8f,      // Darker when pressed
            selectedColor = newColor,
            disabledColor = Color.gray,
            colorMultiplier = 1f
        };
    }

    public void NetworkChangeScene()
    {
       
        NetworkManager.Singleton.SceneManager.LoadScene("Test Level", LoadSceneMode.Single);
        GameManager.Instance.isUI = false;
    }

   



}
