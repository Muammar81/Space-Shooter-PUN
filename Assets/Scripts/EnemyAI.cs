using UnityEngine;
using Photon.Pun;

public class EnemyAI : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float stoppingDistance = 5;

    [Header("Remote Smoothign")]
    [SerializeField] private float remoteRotSmooth = 10;
    [SerializeField] private float remoteMoveSmooth = 5;
    private Vector3 otherPlayerPosition;
    private Quaternion otherPlayerRotation;

    private float rotationOffset = -90;
    private Transform player;

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
}
