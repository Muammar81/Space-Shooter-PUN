using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using System.Collections;

public class EnemyAI : MonoBehaviourPunCallbacks, IPunObservable, IPunEventReceiver
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float stoppingDistance = 5;

    [Header("Remote Smoothing")]
    [SerializeField] private float remoteRotSmooth = 10;
    [SerializeField] private float remoteMoveSmooth = 5;
    private Vector3 otherPlayerPosition;
    private Quaternion otherPlayerRotation;
    private float rotationOffset = -90;

    private PlayerMovement[] allPlayers;
    private Transform targetPlayer;

    private void OnEnable() => PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    private void OnDisable() => PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        PickTargetPlayer();
    }

    void Update()
    {
        if (targetPlayer == null) return;

        if (photonView.IsMine)
            Move();
        else
            MoveNetwork();
    }

    private void PickTargetPlayer()
    {
        allPlayers = FindObjectsOfType<PlayerMovement>();

        var index =  Random.Range(0, allPlayers.Length - 1);
        targetPlayer = allPlayers[index].transform;
    }

    private void Move()
    {
        var direction = (targetPlayer.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(Vector3.forward * (targetAngle + rotationOffset));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        var currentDistance = (targetPlayer.position - transform.position).sqrMagnitude;
        if (currentDistance > stoppingDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
    private void MoveNetwork()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, otherPlayerRotation, Time.deltaTime * remoteRotSmooth);
        transform.position = Vector3.Lerp(transform.position, otherPlayerPosition, Time.deltaTime * remoteMoveSmooth);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            otherPlayerPosition = (Vector3)stream.ReceiveNext();
            otherPlayerRotation = (Quaternion)stream.ReceiveNext();
        }
    }




    //Received by all enemies in the scene
    public void NetworkingClient_EventReceived(EventData e)
    {
        if (e.Code != (byte)PunEventHelper.PunEvents.PLAYER_SPAWNED)
            return;

        object[] dataPacket = (object[])e.CustomData;
        if (dataPacket == null) return;

        var playerVid = (int) dataPacket[0];
        var playerNick = dataPacket[1].ToString();

        var pv = PhotonView.Find(playerVid);
        if(pv.TryGetComponent(out PlayerMovement p))
        {
            //print($"ID: {playerVid}, Name: {playerNick} was spawned");

            //if(Random.Range(0,100)>=50)
                targetPlayer = pv.transform;
        }
    }

    //Callbacks
    public override void OnPlayerEnteredRoom(Player newPlayer) => Debug.Log($"{newPlayer.NickName} joined. Total in room:{PhotonNetwork.CurrentRoom.PlayerCount}");
    public override void OnPlayerLeftRoom(Player otherPlayer) => Debug.Log($"{otherPlayer.NickName} left. Total in room:{PhotonNetwork.CurrentRoom.PlayerCount}");
}


