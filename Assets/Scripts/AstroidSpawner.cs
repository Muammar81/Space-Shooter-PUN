using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class AstroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject astroidPrefab;
    [SerializeField] private float spawnDelay = 2f;
    private float timer;

    private Collider2D _collider;
    private static ObjectPool<GameObject> pool;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        _collider.isTrigger = true;

        pool = new ObjectPool<GameObject>(() =>
        Init(astroidPrefab),
        obj => obj.SetActive(true),
        obj => obj.SetActive(false),
        obj => Destroy(obj), false, 20
        );
    }

    public static void ReturnToPool(GameObject _gameObject)
    {
            pool.Release(_gameObject);
    }

    private GameObject Init(GameObject astroidPrefab)
    {
        var astroid = Instantiate(astroidPrefab);
        astroid.transform.parent = this.transform;
        return astroid;
    }

    void Update()
    {
        if (CanSpawn)
        {
            timer = 0;
            Spawn();
        }
        timer += Time.deltaTime;
    }

    private bool CanSpawn => timer >= spawnDelay;
    private void Spawn()
    {
        var astroid =pool.Get();
        astroid.transform.position = GetRandomPos();
    }

    private Vector3 GetRandomPos()
    {
        float offsetX = Random.Range(-_collider.bounds.extents.x, _collider.bounds.extents.x);
        float offsetY = Random.Range(-_collider.bounds.extents.y, _collider.bounds.extents.y);

        return _collider.bounds.center + new Vector3(offsetX, offsetY, 0);
    }
}
