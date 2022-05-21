using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class PUN_EventHandle : MonoBehaviour, IPunEventReceiver
{
    private void OnEnable() => PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    private void OnDisable() => PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;

    public void NetworkingClient_EventReceived(EventData e)
    {
        if (e.Code == (byte)PunEventHelper.PunEvents.PLAYER_SPAWNED)
        {
            object[] dataPacket = (object[])e.CustomData;
        }
    }
}




