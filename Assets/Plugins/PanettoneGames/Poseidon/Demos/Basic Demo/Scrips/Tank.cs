using UnityEngine;

public class Tank : MonoBehaviour
{
    [SerializeField] private GameObjectPool projectilePool;//1
    [SerializeField] private Transform firePoint;
    private float fireTimer = 1f;
    private float timer;

    private void Awake() => projectilePool.PreWarm();//2
    void Update()
    {
        //shooting
        if (timer > fireTimer) Shoot();
        timer += Time.deltaTime;
    }

    private void Shoot()
    {
        timer = 0;
        var shot = projectilePool.Get();//3
        shot.transform.position = firePoint.transform.position;
        shot.transform.rotation = firePoint.transform.rotation;
        shot.gameObject.SetActive(true);
    }
}
