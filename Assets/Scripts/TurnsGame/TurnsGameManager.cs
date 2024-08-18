using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using UnityEngine.UI;

public class TurnsGameManager : MonoBehaviourPunCallbacks
{
    [Header("Players")]
    public string playerPrefabLocation;
    public TurnsPlayerManager[] players;
    private int playersInGame = 0;
    private int currentPlayerTurn = 1;
    public Button actionButton;
    private TurnsPlayerManager localPlayer;

    public static TurnsGameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        players = new TurnsPlayerManager[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
        
    }

    [PunRPC]
    void ImInGame()
    {
        playersInGame++;

        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            SpawnPlayer();
        }
        
    }

    void SpawnPlayer()
    {
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefabLocation, Vector3.zero, Quaternion.identity);
        TurnsPlayerManager playerScript = playerObj.GetComponent<TurnsPlayerManager>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);

        if (playerScript.photonView.IsMine)
        {
            localPlayer = playerScript;
            actionButton.onClick.AddListener(localPlayer.IncrementScore);
        }
    }

    public void StartTurn(int playerId)
    {
        GetPlayer(playerId)?.photonView.RPC("SetTurn", RpcTarget.All, true);
    }

    public void NextTurn()
    {
        currentPlayerTurn = currentPlayerTurn % playersInGame + 1;
        Debug.Log(currentPlayerTurn);
        StartTurn(currentPlayerTurn);
    }

    public TurnsPlayerManager GetPlayer(int playerId)
    {
        return players.FirstOrDefault(x => x != null && x.id == playerId);
    }

    public TurnsPlayerManager GetPlayer(GameObject playerObj)
    {
        return players.FirstOrDefault(x => x != null && x.gameObject == playerObj);
    }
}
