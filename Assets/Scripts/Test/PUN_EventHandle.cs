using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using UnityEngine;

public class PUN_EventHandle : MonoBehaviour
{
    private void OnEnable()
    {
        string eventName = Enum.GetName(typeof(MyNetworkEvents), MyNetworkEvents.SEND_TEXT);
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable() => PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)MyNetworkEvents.SEND_TEXT)
        {
            object[] dataPacket = (object[])obj.CustomData;
            string playerNickName = (string)dataPacket[0].ToString();
            string txt = (string)dataPacket[1].ToString();
            Debug.Log($"{txt} Received from {playerNickName}");
        }
    }
}
