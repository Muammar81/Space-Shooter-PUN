using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using SpaceShooter.Events;
using System;

public class PUNMainMenu : MonoBehaviourPunCallbacks
{
    private const string PLAYER_NAME = "PLAYER_NAME";

    private void OnEnable()
    {
        base.OnEnable();
        MenuEvents.OnStartGame += Handle_OnStartGame;
    }

    private void OnDisable()
    {
        base.OnDisable();
        MenuEvents.OnStartGame -= Handle_OnStartGame;
    }

    private void Handle_OnStartGame(string playerName)
    {
        Debug.Log($"Connecting as {playerName}...");
        PlayerPrefs.SetString(PLAYER_NAME, playerName);
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected. Joining Lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Lobby joined, Joining Room...");
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene(1);
    }
}
