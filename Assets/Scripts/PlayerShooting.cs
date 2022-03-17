using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerShooting : ShootingBehaviour
{
    [SerializeField] private InputActionReference fireButton;
    [SerializeField] private bool continousShooting;

    private void OnEnable() => fireButton.action.performed += ctx => Fire();
    private void OnDisable() => fireButton.action.performed -= ctx => Fire();

    private void Update()
    {
        if (CanFire)
        {
            timer = 0;
            Fire();
        }
        timer += Time.deltaTime;
    }
    private bool CanFire =>
            fireButton.action.IsPressed() && timer >= fireDelay;

}

