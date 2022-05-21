using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    [SerializeField] List<PhotonView> playerPrefabs;
    [SerializeField] PhotonView enemyPrefab;

    private GameObject playerObject;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected) return;

        SpawnPlayer();

        if (PhotonNetwork.IsMasterClient)
        {
            SpawnEnemies(3);
        }
    }

    private void SpawnEnemies(int enemiesCount)
    {
        for (int i = 0; i < enemiesCount; i++)
        {
            var enemyObject = PhotonNetwork.InstantiateRoomObject(enemyPrefab.name, transform.position, transform.rotation);
            enemyObject.transform.position = Random.insideUnitCircle * 5;
        }
    }

    private void SpawnPlayer()
    {
        Debug.Log("Spawning Player");
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        var playerPrefab = playerPrefabs[playerCount-1]; //careful for number of players
        playerObject = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, transform.rotation);
        playerObject.transform.SetParent(transform);

        var player = playerObject.GetPhotonView().Owner;
        playerObject.transform.name = $"{player.NickName} - Player ({playerCount})";

        if (player.IsLocal)
        {
            playerObject.transform.name += " - Mine";
            PunEventHelper.RiseEvent( PunEventHelper.PunEvents.PLAYER_SPAWNED);
        }

        #if UNITY_EDITOR
        Selection.activeGameObject = playerObject;
        #endif
    }
}







