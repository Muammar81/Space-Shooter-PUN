using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class AstroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject astroidPrefab;
    [SerializeField] private int preWarmAmount;
    [SerializeField] [Tooltip("in Seconds")] private float spawnDelay = 3f;

    private static ObjectPool<GameObject> pool;
    private Collider2D _collider;
    private float timer;

    private void Start()
    {
        pool = new ObjectPool<GameObject>(() =>
        Init(astroidPrefab),
        obj => obj.gameObject.SetActive(true),
        obj => obj.gameObject.SetActive(false),
        obj => Destroy(obj.gameObject),
        false, preWarmAmount);

        _collider= GetComponent<Collider2D>();
        _collider.isTrigger = true;
    }

    private GameObject Init(GameObject astroidPrefab)
    {
        var astroid = Instantiate(astroidPrefab);
        astroid.transform.parent = this.transform;
        return astroid;
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


    private void Update()
    {
        if (CanCanSpawn)
        {
            timer = 0;
            Spawn();
        }

        timer += Time.deltaTime;
    }

    private bool CanCanSpawn => timer >= spawnDelay;

    private void Spawn()
    {
        var astroid = pool.Get();
        astroid.transform.position = GetRandomPosition();
        astroid.transform.eulerAngles = GetRandomRotation();
    }
    private Vector3 GetRandomPosition()
    {
        float offsetX = Random.Range(-_collider.bounds.extents.x, _collider.bounds.extents.x);
        float offsetY = Random.Range(-_collider.bounds.extents.y, _collider.bounds.extents.y);

        return _collider.bounds.center + new Vector3(offsetX, offsetY, 0);
    }

    private Vector3 GetRandomRotation()
    {
        Vector3 euler = transform.eulerAngles;
        euler.z = Random.Range(-70f, -130f);
        return euler;
    }

    public void OnDrawGizmos()
    {
        if (!_collider)
            return;

        var offset = _collider.offset;
        var extents = _collider.bounds.size * 0.5f;
        var verts = new Vector2[] {
            transform.TransformPoint (new Vector2 (-extents.x, -extents.y) + offset),
            transform.TransformPoint (new Vector2 (extents.x, -extents.y) + offset),
            transform.TransformPoint (new Vector2 (extents.x, extents.y) + offset),
            transform.TransformPoint (new Vector2 (-extents.x, extents.y) + offset) };
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_collider.bounds.center, _collider.bounds.size);

    }

}
