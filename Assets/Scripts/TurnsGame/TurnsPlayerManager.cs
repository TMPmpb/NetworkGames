using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TurnsPlayerManager : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public int score = 0;

    [Header("Components")]
    public Player photonPlayer;

    [HideInInspector]
    public bool isMyTurn = false;

    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;

        TurnsGameManager.instance.players[id - 1] = this;
        Debug.Log("Player: " + id);
        if (id == 1)
        {
            TurnsGameManager.instance.StartTurn(id);
        }
    }

    void Update()
    {
        /*if (isMyTurn)
        {
            Debug.Log(score);
        }
        if (photonView.IsMine && isMyTurn)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                IncrementScore();
                EndTurn();
            }
        }*/
    }

    public void IncrementScore()
    {
        if (photonView.IsMine && isMyTurn)
        {
            score += 1;
            Debug.Log(photonPlayer.NickName + ": " + score);
            photonView.RPC("UpdateScoreUI", RpcTarget.All, score);
            EndTurn();
        }
    }

    [PunRPC]
    public void UpdateScoreUI(int newScore)
    {
        // Actualiza la UI del jugador con el nuevo puntaje
    }

    void EndTurn()
    {
        isMyTurn = false;
        photonView.RPC("NextTurn", RpcTarget.All);
    }

    [PunRPC]
    public void SetTurn(bool turn)
    {
        isMyTurn = turn;
    }

    [PunRPC]
    public void NextTurn()
    {
        TurnsGameManager.instance.NextTurn();
    }
}
