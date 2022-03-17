using UnityEngine;
using PanettoneGames;

public class EnemyShooting : ShootingBehaviour
{
    [SerializeField] private GameObjectPool pool;
    private void Awake() => pool.Prewarm();
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
    protected void Fire()
    {
        for (int i = 0; i < firePoints.Count; i++)
        {
            var bullet = pool.Get();
            bullet.transform.position = firePoints[i].position;
            bullet.transform.rotation = firePoints[i].rotation;
        }
        OnFire?.Invoke(sFX);
    }
}

