using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<PhotonView> playerPrefabs;
    [SerializeField] private PhotonView enemyPrefab;

    private GameObject playerObject;

    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        SpawnPlayer();

        if (PhotonNetwork.IsMasterClient)
            SpawnEnemies(3);
    }

    private void SpawnEnemies(int enemiesCount)
    {
        for (int i = 0; i < enemiesCount; i++)
        {
            GameObject enemyObject = PhotonNetwork.InstantiateRoomObject(enemyPrefab.name, transform.position, transform.rotation);
            enemyObject.transform.position = Random.insideUnitCircle * 5;
        }
    }

    private void SpawnPlayer()
    {
        Debug.Log("Spawning Player");
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonView playerPrefab = playerPrefabs[playerCount - 1]; //careful for number of players
        playerObject = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, transform.rotation);
        playerObject.transform.SetParent(transform);

        var player = playerObject.GetPhotonView();
        playerObject.transform.name = $"{player.name} - Player ({playerCount})";

        if (player.IsMine)
        {
            playerObject.transform.name += " - Mine";

            object[] dataPacket = new object[] { player.ViewID, PhotonNetwork.NickName};
            PunEventHelper.RiseEvent(PunEventHelper.PunEvents.PLAYER_SPAWNED, dataPacket);
        }

        #if UNITY_EDITOR
        Selection.activeGameObject = playerObject;
        #endif
    }
}