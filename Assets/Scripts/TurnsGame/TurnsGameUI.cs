using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class TurnsGameUI : MonoBehaviour
{
    public PlayerTurnsUIContainer[] playerContainers;
    public TextMeshProUGUI currentPlayer;

    public static TurnsGameUI instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializePlayerUI();
    }
    private void Update()
    {
        UpdatePlayerUI();
    }

    void InitializePlayerUI()
    {
        for (int x = 0; x < playerContainers.Length; x++)
        {
            PlayerTurnsUIContainer container = playerContainers[x];

            if (x < PhotonNetwork.PlayerList.Length)
            {
                container.obj.SetActive(true);
                container.nameText.text = PhotonNetwork.PlayerList[x].NickName;
                container.playerNumber.text = "";
            }
            else
            {
                container.obj.SetActive(false);
            }
        }
    }

    void UpdatePlayerUI()
    {
        for (int x = 0; x < GameManager.instance.players.Length; x++)
        {
            Debug.Log(GameManager.instance.players[x]);
            if (GameManager.instance.players[x] != null)
            {
                playerContainers[x].playerNumber.text = "";
            }
        }
    }

    public void SetCurrentPlayerText(string playerName)
    {
        currentPlayer.text = "Turno de "+playerName;
    }
}

[System.Serializable]
public class PlayerTurnsUIContainer
{
    public GameObject obj;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI playerNumber;
}