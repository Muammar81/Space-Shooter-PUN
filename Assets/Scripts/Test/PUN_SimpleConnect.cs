using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class PUN_SimpleConnect : MonoBehaviourPunCallbacks
{
    private void Awake() => PhotonNetwork.ConnectUsingSettings();
    public override void OnConnectedToMaster() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby() => PhotonNetwork.JoinRandomOrCreateRoom();
    public override void OnJoinedRoom()
    {
        int playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
        string nick = $"Player {playersCount}";
        PhotonNetwork.NickName = nick;
        print($"Connected as {nick}.");
        //SceneManager.LoadScene(1);
    }
}