using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;


// States the player can have on the game
public enum ConnectionState : byte
{
    connected,
    disconnected,
    ready
}

// Struct for better serialization on the player connection
[Serializable]
public struct PlayerConnectionState
{
    public ConnectionState playerState;             // State of the player
    public PlayerCharSelection playerObject;        // The NetworkObject of the client use for the disconnection of the client
    public string playerName;                       // The name of the player when spawn
    public ulong clientId;                          // Id of the client
    public int idOfCharSelectd;
}

// Struct for better serialization on the container of the character
[Serializable]
public struct CharacterContainer
{
    public GameObject imageContainer;                    // The image of the character container
    public GameObject nameContainer;           // Character name container
    public GameObject iconReady;                  // The border of the character container when ready
    public GameObject waitingText;                  // The waiting text on the container were no client connected

}

public class CharacterSelectionManager : SingletonNetwork<CharacterSelectionManager>
{
    public CharacterDatabase charDB;
    public CharacterDataSO[] charactersData;
    [SerializeField]
    CharacterContainer[] m_charactersContainers;

    [SerializeField]
    GameObject m_readyButton;

    [SerializeField]
    GameObject m_cancelButton;

    [SerializeField]
    float m_timeToStartGame;

    [SerializeField]
    string m_nextScene = "GameMultiplay";

    [SerializeField]
    PlayerConnectionState[] m_playerStates;

    [SerializeField]
    GameObject m_playerPrefab;

    [Header("Audio clips")]
    [SerializeField]
    AudioClip m_confirmClip;

    [SerializeField]
    AudioClip m_cancelClip;


    bool m_isTimerOn;
    float m_timer;


    void Start()
    {
        m_timer = m_timeToStartGame;

    }

    void Update()
    {
        if (!IsServer)
            return;

        if (!m_isTimerOn)
            return;

        m_timer -= Time.deltaTime;
        if (m_timer <= 0f)
        {
            m_isTimerOn = false;
            StartGame();
        }
    }



    void StartGame()
    {
        StartGameClientRpc();
        LoadingSceneManager.Instance.LoadScene(m_nextScene);
    }

    [ClientRpc]
    void StartGameClientRpc()
    {
        LoadingFadeEffect.Instance.FadeAll();
    }


    void RemoveReadyStates(ulong clientId, bool disconected)
    {
        for (int i = 0; i < m_playerStates.Length; i++)
        {
            if (m_playerStates[i].playerState == ConnectionState.ready &&
                m_playerStates[i].clientId == clientId)
            {

                if (disconected)
                {
                    m_playerStates[i].playerState = ConnectionState.disconnected;
                    UpdatePlayerStateClientRpc(clientId, i, ConnectionState.disconnected);
                }
                else
                {
                    m_playerStates[i].playerState = ConnectionState.connected;
                    UpdatePlayerStateClientRpc(clientId, i, ConnectionState.connected);
                }
            }
        }
    }

    [ClientRpc]
    void UpdatePlayerStateClientRpc(ulong clientId, int stateIndex, ConnectionState state)   // keep
    {
        if (IsServer)
            return;

        m_playerStates[stateIndex].playerState = state;
        m_playerStates[stateIndex].clientId = clientId;
    }

    void StartGameTimer()
    {

        int count = 0;


        foreach (PlayerConnectionState state in m_playerStates)
        {
            // If a player is connected (not ready)
            if (state.playerState == ConnectionState.connected)
                return;
            else if (state.playerState == ConnectionState.ready) // count the number of alread ready
                count++;

        }

        if (count < 2)  //must have 2 player in scene
            return;

        // If all players connected are ready
        m_timer = m_timeToStartGame;
        m_isTimerOn = true;
    }


    void SetNonPlayableChar(int playerId)  // display empty slot
    {
        m_charactersContainers[playerId].imageContainer.SetActive(false);
        m_charactersContainers[playerId].nameContainer.SetActive(false);
        m_charactersContainers[playerId].waitingText.SetActive(true);
        m_charactersContainers[playerId].iconReady.SetActive(false);
    }



    public void SetPlayebleChar(int playerId, int characterSelected, bool isClientOwner)
    {
        SetCharacterUI(playerId, characterSelected);
        //m_charactersContainers[playerId].playerIcon.gameObject.SetActive(true);
        //if (isClientOwner)
        //{
        //    m_charactersContainers[playerId].borderClient.SetActive(true);
        //    m_charactersContainers[playerId].border.SetActive(false);
        //    m_charactersContainers[playerId].borderReady.SetActive(false);
        //    m_charactersContainers[playerId].playerIcon.color = m_clientColor;
        //}
        //else
        //{
        //    m_charactersContainers[playerId].border.SetActive(true);
        //    m_charactersContainers[playerId].borderReady.SetActive(false);
        //    m_charactersContainers[playerId].borderClient.SetActive(false);
        //    m_charactersContainers[playerId].playerIcon.color = m_playerColor;
        //}

        //m_charactersContainers[playerId].backgroundShip.SetActive(true);

        m_charactersContainers[playerId].imageContainer.SetActive(true);
        m_charactersContainers[playerId].nameContainer.SetActive(true);
        m_charactersContainers[playerId].imageContainer.GetComponent<Image>().sprite = charDB.GetCharacter(characterSelected).characterSprite;
        m_charactersContainers[playerId].nameContainer.GetComponent<TextMeshProUGUI>().text = charDB.GetCharacter(characterSelected).characterName;
        m_charactersContainers[playerId].iconReady.SetActive(false);
        m_charactersContainers[playerId].waitingText.SetActive(false);
    }

    public void SetCharacterUI(int playerId, int characterSelected)
    {

        m_charactersContainers[playerId].imageContainer.GetComponent<Image>().sprite = charDB.GetCharacter(characterSelected).characterSprite;
        m_charactersContainers[playerId].nameContainer.GetComponent<TextMeshProUGUI>().text = charDB.GetCharacter(characterSelected).characterName;
    }

    public ConnectionState GetConnectionState(int playerId)  // Return current state of player
    {
        if (playerId != -1)
            return m_playerStates[playerId].playerState;

        return ConnectionState.disconnected;
    }

    public void ServerSceneInit(ulong clientId)
    {
        GameObject go =
            NetworkObjectSpawner.SpawnNewNetworkObjectChangeOwnershipToClient(
                m_playerPrefab,
                transform.position,
                clientId,
                true);

        for (int i = 0; i < m_playerStates.Length; i++)
        {
            if (m_playerStates[i].playerState == ConnectionState.disconnected)
            {
                m_playerStates[i].playerState = ConnectionState.connected;
                m_playerStates[i].playerObject = go.GetComponent<PlayerCharSelection>();
                m_playerStates[i].playerName = go.name;
                m_playerStates[i].clientId = clientId;

                // Force the exit
                break;
            }
        }

        // Sync states to clients
        for (int i = 0; i < m_playerStates.Length; i++)
        {
            if (m_playerStates[i].playerObject != null)
                PlayerConnectsClientRpc(
                    m_playerStates[i].clientId,
                    i,
                    m_playerStates[i].playerState,
                    m_playerStates[i].playerObject.GetComponent<NetworkObject>());
        }

    }

    [ClientRpc]
    void PlayerConnectsClientRpc(
        ulong clientId,
        int stateIndex,
        ConnectionState state,
        NetworkObjectReference player)
    {
        if (IsServer)
            return;

        if (state != ConnectionState.disconnected)
        {

            m_playerStates[stateIndex].playerState = state;
            m_playerStates[stateIndex].clientId = clientId;

            if (player.TryGet(out NetworkObject playerObject))
                m_playerStates[stateIndex].playerObject =
                    playerObject.GetComponent<PlayerCharSelection>();
        }
    }
    // Keep
    public void PlayerDisconnects(ulong clientId)
    {
        if (!ClientConnection.Instance.IsExtraClient(clientId))
            return;

        PlayerNotReady(clientId, isDisconected: true);

        m_playerStates[GetPlayerId(clientId)].playerObject.Despawn();

        // The client disconnected is the host
        if (clientId == 0)
        {
            NetworkManager.Singleton.Shutdown();
        }
    }
    // edit lai 
    public void PlayerNotReady(ulong clientId, int characterSelected = 0, bool isDisconected = false)
    {
        int playerId = GetPlayerId(clientId);
        m_isTimerOn = false;
        m_timer = m_timeToStartGame;

        RemoveReadyStates(clientId, isDisconected);

        // Notify clients to change UI
        if (isDisconected)
        {
            PlayerDisconnectedClientRpc(playerId);
        }
        else
        {
            PlayerNotReadyClientRpc(clientId, playerId, characterSelected);
        }
    }
    // Edit lai 
    public int GetPlayerId(ulong clientId)
    {
        for (int i = 0; i < m_playerStates.Length; i++)
        {
            if (m_playerStates[i].clientId == clientId)
                return i;
        }

        //! This should never happen
        Debug.LogError("This should never happen");
        return -1;
    }

    // Set the player ready if the player is not selected and check if all player are ready to start the countdown
    // Keep
    public void PlayerReady(ulong clientId, int playerId, int characterSelected)   //  keep
    {

        int count = 0;


        foreach (PlayerConnectionState state in m_playerStates)
        {
            // If a player is connected (not ready)
            if ((state.playerState == ConnectionState.connected) || (state.playerState == ConnectionState.ready)) // count the number of a lread ready

                count++;

        }

        if (count < 2)  //must have 2 player in scene
        {
            StartReadyUIButtons();
            return;
        }


        PlayerReadyClientRpc(clientId, playerId, characterSelected);

        StartGameTimer();
    }

    // Set the players UI button --- Keep
    public void SetPlayerReadyUIButtons(bool isReady, int characterSelected)
    {
        //if (isReady && !charactersData[characterSelected].isSelected)
        if (isReady)
        {
            m_readyButton.SetActive(false);
            m_cancelButton.SetActive(true);
        }
        else /* else if (!isReady && charactersData[characterSelected].isSelected)*/
        {
            m_readyButton.SetActive(true);
            m_cancelButton.SetActive(false);
        }
    }

    public void StartReadyUIButtons()
    {

        m_readyButton.SetActive(true);
        m_cancelButton.SetActive(false);

    }

    // Check if the player has selected the character
    //public bool IsSelectedByPlayer(int playerId, int characterSelected)
    //{

    //    //return charactersData[characterSelected].playerId == playerId ? true : false;
    //    return false;
    //}

    [ClientRpc]
    void PlayerReadyClientRpc(ulong clientId, int playerId, int characterSelected)
    {
        m_playerStates[playerId].playerState = ConnectionState.ready;
        m_charactersContainers[playerId].iconReady.SetActive(true);
        m_playerStates[playerId].idOfCharSelectd = characterSelected;
        if(playerId == 0)
        {
            charactersData[0].clientId = clientId;
            charactersData[0].charId = characterSelected;
            charactersData[0].playerId = playerId;
        }
        else if (playerId == 1) 
        {
            charactersData[1].clientId = clientId;
            charactersData[1].charId = characterSelected;
            charactersData[1].playerId = playerId;
        }
    }

    // Send characterData
    //public void DeliverClientInfo(ulong clientId, int playerId, int characterSelected)
    //{
    //    SendInfoServerRpc(clientId,playerId,characterSelected);
    //}
    //[ServerRpc]
    //void SendInfoServerRpc(ulong clientId, int playerId, int characterSelected)
    //{
    //    if (IsHost)
    //    {
    //        charactersData[0].clientId = clientId;
    //        charactersData[0].charId = characterSelected;
    //        charactersData[0].playerId = playerId;
    //    }

    //    else if (IsClient)
    //    {
    //        charactersData[1].clientId = clientId;
    //        charactersData[1].charId = characterSelected;
    //        charactersData[1].playerId = playerId;
    //    }
    //}


    [ClientRpc]
    void PlayerNotReadyClientRpc(ulong clientId, int playerId, int characterSelected)
    {
        m_charactersContainers[playerId].iconReady.SetActive(false);
    }

    [ClientRpc]
    public void PlayerDisconnectedClientRpc(int playerId)
    {
        SetNonPlayableChar(playerId);

        m_playerStates[playerId].playerState = ConnectionState.disconnected;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += PlayerDisconnects;
        }
    }

    void OnDisable()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= PlayerDisconnects;
        }
    }


}
