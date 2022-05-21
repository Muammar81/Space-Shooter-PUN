using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PUN_EventRaise : MonoBehaviourPun
{
    [ContextMenu("Send Event")]
    private void SendEvent()
    {
        string txt = "Test " + Random.Range(100, 999).ToString();

        //Print locally
        print($"Sending {txt}...");

        //send to others
        object[] dataPacket = new object[] { PhotonNetwork.NickName, txt };
        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };

        PhotonNetwork.RaiseEvent((byte)PunEventHelper.PunEvents.PLAYER_SPAWNED, dataPacket, eventOptions, SendOptions.SendReliable);
    }
}


