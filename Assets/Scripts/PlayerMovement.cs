using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] InputActionReference leftStick;
    //[SerializeField] InputActionReference fireButton;
    private float moveSpeed = 5f;

    //private void OnEnable() => fireButton.action.performed += OnFire;
    //private void OnDisable() => fireButton.action.performed -= OnFire;

    public void Update()
    {
        var move = leftStick.action.ReadValue<Vector2>();
        Move(move);
    }

    private void Move(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.01)
            return;

        var move =  new Vector3(- direction.x, direction.y, 0);
        transform.position += move * moveSpeed * Time.deltaTime;
    }

    //private void OnFire(InputAction.CallbackContext ctx) => print($"fire{ctx.action.name}");
}
