using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private InputActionReference fireButton;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int preWarmAmount;
    [SerializeField] private List<Transform> firePoints;
    [SerializeField] private bool continousShooting;
    [SerializeField] private float fireDelay = 02f;
    [SerializeField] private AudioClip sFX;


    private static ObjectPool<GameObject> pool;
    private AudioSource audiouSource;
    private float timer;

    private void OnEnable() => fireButton.action.performed += ctx => Fire();
    private void OnDisable() => fireButton.action.performed -= ctx => Fire();

    private void Start()
    {
        pool = new ObjectPool<GameObject>(() =>
        Init(bulletPrefab),
        obj => obj.gameObject.SetActive(true),
        obj => obj.gameObject.SetActive(false),
        obj => Destroy(obj.gameObject),
        false,preWarmAmount);

        audiouSource = GetComponent<AudioSource>();
        audiouSource.playOnAwake = false;
    }
    private void Update()
    {
        if (continousShooting && CanFire)
        {
            timer = 0;
            Fire();
        }

        timer += Time.deltaTime;
    }

    private bool CanFire=> 
        fireButton.action.IsPressed() && 
        timer >= fireDelay;

    private GameObject Init(GameObject bulletPrefab)
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.transform.parent = this.transform;
        return bullet;
    }

    /// <summary>
    /// Returns Pooled Object to Pool
    /// </summary>
    /// <param name="objectToReturn">Object to return</param>
    /// <param name="delay">delay in milliseconds</param>
    internal static async void ReturnToPool(GameObject objectToReturn, float delay = 0)
    {
        if (delay > 0)
        {
            //Disable Visuals
            var rends = objectToReturn.GetComponents<Renderer>().ToList();
            rends.ForEach(r => r.enabled = false);

            var endTime = Time.time + delay;
            while (Time.time < endTime)
            {
                await Task.Yield();
            }
        }
        pool.Release(objectToReturn);
    }

    private void Fire()
    {
        audiouSource.PlayOneShot(sFX);
        for (int i = 0; i < firePoints.Count; i++)
        {
            var bullet = pool.Get();
            bullet.transform.position = firePoints[i].position;
            bullet.transform.rotation = firePoints[i].rotation;
        }
    }
}

