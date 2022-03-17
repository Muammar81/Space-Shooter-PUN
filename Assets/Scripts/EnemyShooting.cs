using UnityEngine;
public class EnemyShooting : ShootingBehaviour
{
    private void Update()
    {
        if (CanFire)
        {
            timer = 0;
            Fire();
        }
        timer += Time.deltaTime;
    }
    private bool CanFire => timer >= fireDelay;
}

