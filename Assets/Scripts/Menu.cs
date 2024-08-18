using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Screens")]
    public GameObject gameSelect;
    

    [Header("Get the hat game")]
    public GameObject getTheHatGame;
    public GameObject getTheHatLobbyScreen;
    public Button getTheHatCreateRoomButton;
    public Button getTheHatJoinRoomButton;
    public TextMeshProUGUI getTheHatPlayerListText;
    public Button getTheHatStartGameButton;

    [Header("Turns game")]
    public GameObject turnsGame;
    public GameObject turnsGameLobbyScreen;
    public Button turnsCreateRoomButton;
    public Button turnsJoinRoomButton;
    public TextMeshProUGUI turnsPlayerListText;
    public Button turnsStartGameButton;
    void Start()
    {
        //getTheHatCreateRoomButton.interactable = false;
        //getTheHatJoinRoomButton.interactable = false;
        ManageButtons(false);
    }

    public override void OnConnectedToMaster()
    {
        ManageButtons(true);
        //getTheHatCreateRoomButton.interactable = true;
        //getTheHatJoinRoomButton.interactable = true;
    }

    void SetScreen(GameObject screen)
    {
        //getTheHatGame.SetActive(false);
        //getTheHatLobbyScreen.SetActive(false);
        SetGameScreensState(false);
        screen.SetActive(true);
    }

    public void OnCreateRoomButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    public void OnJoinRoomButton(TMP_InputField roomNameInput)
    {
        NetworkManager.instance.JoinRoom(roomNameInput.text);
    }

    public void OnPlayerNameUpdate(TMP_InputField playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text;
    }

    public override void OnJoinedRoom()
    {
        //SetScreen(getTheHatLobbyScreen);
        SetScreen(GetCurrentGameScreen());
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateLobbyUI();
    }

    [PunRPC]
    public void UpdateLobbyUI()
    {
        getTheHatPlayerListText.text = "";
        turnsPlayerListText.text = "";

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            getTheHatPlayerListText.text += player.NickName+"\n";
            turnsPlayerListText.text += player.NickName + "\n";
        }

        //only the host
        if (PhotonNetwork.IsMasterClient)
        {
            getTheHatStartGameButton.interactable = true;
        }
        else
        {
            getTheHatStartGameButton.interactable = false;
        }
    }

    public void GoBackToGameSelect()
    {
        gameSelect.SetActive(true);
        getTheHatGame.SetActive(false);
        turnsGame.SetActive(false);
    }

    public void ShowHatGame()
    {
        gameSelect.SetActive(false);
        getTheHatGame.SetActive(true);
    }

    public void ShowTurnsGame()
    {
        gameSelect.SetActive(false);
        turnsGame.SetActive(true);
    }

    public void OnLeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(GetCurrentGameScreen());
        //SetScreen(getTheHatGame);
    }

    public void OnStartGameButton()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }

    public void OnStartGameTurnsButton()
    {
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "TestTurns");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public GameObject GetCurrentGameScreen()
    {
        if (getTheHatGame.activeSelf)
        {
            return getTheHatLobbyScreen;
        }
        else if(getTheHatLobbyScreen.activeSelf)
        {
            return getTheHatGame;
        }
        else if(turnsGame.activeSelf)
        {
            return turnsGameLobbyScreen;
        }
        else if (turnsGameLobbyScreen.activeSelf)
        {
            return turnsGame;
        }
        else
        {
            return null;
        }
    }

    private void SetGameScreensState(bool state)
    {
        getTheHatGame.SetActive(state);
        getTheHatLobbyScreen.SetActive(state);
        turnsGame.SetActive(state);
        turnsGameLobbyScreen.SetActive(state);
    }

    private void ManageButtons(bool state)
    {
        getTheHatCreateRoomButton.interactable = state;
        getTheHatJoinRoomButton.interactable = state;
        turnsCreateRoomButton.interactable = state;
        turnsJoinRoomButton.interactable = state;
    }
}
