using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 15f;
    [SerializeField] private float stoppingDistance = 5;
    private float rotationOffset = -90;

    private Transform player;

    private void Start() => player = FindObjectOfType<PlayerMovement>().gameObject.transform;

    void Update()
    {
        if (player == null) return;

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
}
