
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
    public TextMeshProUGUI LobbyCount;


    void Start()
    {
        transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        readyPlayers.Value = 0;
    }

  



    void UpdateUI(ulong clientId)
    {
        LobbyCount.text="Ready: "+readyPlayers.Value+"//"+ GameManager.Instance.GetPlayerGameObjects().Count;
    }

    [ClientRpc]
    private void UpdateReadyCountClientRpc()
    {
        LobbyCount.text = "Ready: " + readyPlayers.Value + "//" + GameManager.Instance.GetPlayerGameObjects().Count; 
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

        
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += UpdateUI;
        }
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
        ChangeButtonColor(Color.green);
        readyUpButton.GetComponentInChildren<TMP_Text>().text = "Ready";

        if (IsServer)
        {
            readyPlayers.Value++;
            UpdateReadyCountClientRpc();
        }
        else
        {
            UpdateReadyCountClientRpc();
        }

    }
    [ServerRpc(RequireOwnership = false)]
    private void AddReadyPlayerServerRpc()
    {
        // If you're the server, directly add to ready players
        readyPlayers.Value++;
        UpdateReadyCountClientRpc();
    }

    void ChangeButtonColor(Color newColor)
    {
        readyUpButton.colors = new ColorBlock
        {
            normalColor = newColor,
            highlightedColor = newColor * 1.2f,  // Slightly brighter on hover
            pressedColor = newColor * 0.8f,      // Darker when pressed
            selectedColor = newColor,
            disabledColor = Color.gray,
            colorMultiplier = 1f
        };
    }

}
