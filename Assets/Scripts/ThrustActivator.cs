using UnityEngine;
using UnityEngine.InputSystem;

public class ThrustActivator : MonoBehaviour
{
    [SerializeField] InputActionReference leftStick;
    [SerializeField] GameObject parentThruster;
    void Update() => parentThruster.SetActive(leftStick.action.ReadValue<Vector2>().x > 0.1f);
}
