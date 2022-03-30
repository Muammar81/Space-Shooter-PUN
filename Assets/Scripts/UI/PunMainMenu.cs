using Photon.Pun;
using Photon.Realtime;
using SpaceShooter.Events;
using System;
using UnityEngine;

public class PUNMainMenu : MonoBehaviourPunCallbacks
{
    private const string PLAYER_NAME = "PLAYER_NAME";
    private string _playerName;

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
        _playerName = playerName;

        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.AutomaticallySyncScene = false;
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
        //PhotonNetwork.JoinRandomOrCreateRoom();

        var roomOptions = new RoomOptions
        {
            MaxPlayers = 8,
            PublishUserId = true,
            IsVisible = true
        };

        //roomOptions.BroadcastPropsChangeToAll = true;
        PhotonNetwork.JoinOrCreateRoom("Default",roomOptions,TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        _playerName = String.IsNullOrEmpty(_playerName) ?
            $"Player {playerCount}" : _playerName;

        PhotonNetwork.NickName = _playerName;
        PlayerPrefs.SetString(PLAYER_NAME, _playerName);
        PhotonNetwork.LoadLevel(1);
    }
}
