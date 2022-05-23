using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviourPun, IPunObservable
{
    [SerializeField] InputActionReference leftStick;
    [SerializeField] float moveSpeed = 5f;

    [Header("Remote Smoothign")]
    [SerializeField] private float remoteRotSmooth = 10;
    [SerializeField] private float remoteMoveSmooth = 5;
    private Vector3 otherPlayerPosition;
    private Quaternion otherPlayerRotation;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            Destroy(gameObject.GetComponent<PlayerInput>());
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            var movement = leftStick.action.ReadValue<Vector2>();
            Move(movement);
        }
        else
        {
            MoveNetwork();
        }
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;

        var move = new Vector3(-direction.x, direction.y, 0);
        transform.position += move * moveSpeed * Time.deltaTime;
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
