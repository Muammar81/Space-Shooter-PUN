using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 15;
    [SerializeField] private float rotationOffset = 90;
    [SerializeField] private float stoppingDistance = 5f;

    private Transform player;
    private void OnEnable()
    {
        var players = FindObjectsOfType<PlayerMovement>().ToList();
        var r = new System.Random();
        int index = r.Next(0, players.Count-1);
        player = players[index].transform;
    }
    void Update()
    {
        var direction = (player.position - transform.position).normalized;

        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle += rotationOffset;
        var targetRotation = Quaternion.Euler(Vector3.forward * (targetAngle + rotationOffset));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        
        var currentDistance = (player.position - transform.position).sqrMagnitude;
        if (currentDistance > stoppingDistance)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
