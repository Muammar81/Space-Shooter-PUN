using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionReference leftStick;
    [SerializeField] float moveSpeed = 5f;
    void Update()
    {
        var movement = leftStick.action.ReadValue<Vector2>();
        Move(movement);
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;

        var move = new Vector3(-direction.x, direction.y, 0);
        transform.position += move * moveSpeed * Time.deltaTime;
    }
}
