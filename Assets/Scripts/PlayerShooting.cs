using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private InputActionReference fireButton;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int preWarmAmount;
    [SerializeField] List<Transform> firePoints;

    private ObjectPool<GameObject> pool;

    private void OnEnable() => fireButton.action.performed += OnShooting;
    private void OnDisable() => fireButton.action.performed -= OnShooting;

    private void Start()
    {
        pool = new ObjectPool<GameObject>(() =>
        Init(bulletPrefab),
        obj => obj.gameObject.SetActive(true),
        obj => obj.gameObject.SetActive(false),
        obj => Destroy(obj.gameObject),
        false, preWarmAmount);
    }

    private GameObject Init(GameObject bulletPrefab)
    {
        var bullet = Instantiate(bulletPrefab);
        return bullet;
    }

    /// <summary>
    /// Returns Pooled Object to Pool
    /// </summary>
    /// <param name="pooledObject">Object to return</param>
    /// <param name="delay">delay in milliseconds</param>
    internal async void ReturnToPool(GameObject pooledObject, float delay = 0)
    {
        if (delay > 0)
        {
            var endTime = Time.time + delay;
            while (Time.time < endTime)
            {
                await Task.Yield();
            }
        }
        pool.Release(pooledObject);
    }



    private void OnShooting(InputAction.CallbackContext obj)
    {
        for (int i = 0; i < firePoints.Count; i++)
        {
            var bullet = pool.Get();
            bullet.transform.position = firePoints[i].position;
            bullet.transform.rotation = firePoints[i].rotation;
        }
    }
}

public interface IGameObjectPooled
{
    PlayerShooting Pool { get; set; }
}
