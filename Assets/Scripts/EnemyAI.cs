using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

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
    public Transform Player { get; set; }

    private void OnEnable() => PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    private void OnDisable() => PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;

    private void Start() => Player = FindObjectOfType<PlayerMovement>().gameObject.transform;

    void Update()
    {
        if (Player == null) return;

        if (photonView.IsMine)
            Move();
        else
            MoveNetwork();
    }

    private void Move()
    {
        var direction = (Player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(Vector3.forward * (targetAngle + rotationOffset));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        var currentDistance = (Player.position - transform.position).sqrMagnitude;
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

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        
        var pv = PhotonView.Find(newPlayer.ActorNumber);
        if (pv.TryGetComponent(out PlayerMovement playerMove))
        {
            print($"Attacking {pv.name}");
            Player = pv.transform;
        }
    }

    //Received by all enemies in the scene
    public void NetworkingClient_EventReceived(EventData e)
    {
        if (e.Code != (byte)PunEventHelper.PunEvents.PLAYER_SPAWNED)
            return;

        object[] dataPacket = (object[])e.CustomData;
        var playerVid = (int) dataPacket[0];
        var playerNick = dataPacket[1].ToString();

        var pv = PhotonView.Find(playerVid);
        if(pv.TryGetComponent(out PlayerMovement p))
        {
            print($"ID: {playerVid}, Name: {playerNick} was spawned");

            if(Random.Range(0,100)>50)
                Player = pv.transform;
        }
    }
}


