using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class EnemyAI : MonoBehaviourPun, IPunObservable, IPunEventReceiver
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
    private Transform player;

    private void OnEnable() => PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    private void OnDisable() => PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    private void Start() => player = FindObjectOfType<PlayerMovement>().gameObject.transform;

    void Update()
    {
        if (player == null) return;

        if (photonView.IsMine)
        {
            Move();
        }
        else
        {
            MoveNetwork();
        }
    }

    private void Move()
    {
        var direction = (player.position - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        var targetRotation = Quaternion.Euler(Vector3.forward * (targetAngle + rotationOffset));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        var currentDistance = (player.position - transform.position).sqrMagnitude;
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

    public void NetworkingClient_EventReceived(EventData e)
    {
        if (e.Code == (byte)PunEventHelper.PunEvents.PLAYER_SPAWNED)
        {
            var pid = e.Sender;
            var pv = PhotonView.Find(pid);

            print($"attacking {pv.Owner.NickName}");
            player = pv.transform;

            //object[] dataPacket = (object[])e.CustomData;
            //print(dataPacket[0]?.ToString());
        }
    }
}


