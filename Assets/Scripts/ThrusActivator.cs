using UnityEngine;
using UnityEngine.InputSystem;

public class ThrusActivator : MonoBehaviour
{
    [SerializeField] InputActionReference leftStick;
    [SerializeField] GameObject thrusters;
    void Update() => thrusters.SetActive(leftStick.action.ReadValue<Vector2>().x > 0.1f);
}
