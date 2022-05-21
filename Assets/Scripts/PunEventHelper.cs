using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PunEventHelper : MonoBehaviour
{
    public enum PunEvents { PLAYER_SPAWNED, Player_DIED }
    public static void RiseEvent(PunEvents e, object[] dataPacket = null)
    {
        RaiseEventOptions eventOptions = new RaiseEventOptions
        {
            CachingOption = EventCaching.DoNotCache,
            Receivers = ReceiverGroup.All
        };
        PhotonNetwork.RaiseEvent((byte)e, dataPacket, eventOptions, SendOptions.SendReliable);
    }

}
public interface IPunEventReceiver
{
    void NetworkingClient_EventReceived(EventData e);
}
