using UnityEngine;
using PanettoneGames;

public class Bullet : MonoBehaviour, PanettoneGames.IGameObjectPooled
{
    [SerializeField] [Range(10, 100)] float launchSpeed = 30f;
    [SerializeField] [Tooltip("In Seconds")] [Range(1, 10)] private float maxLifeTime = 2f;
    [SerializeField] [Tooltip("Remember to turn off particle play on awake")] ParticleSystem FX;
    [SerializeField] [Tooltip("Typically, player's layer")] private LayerMask ignoredLayers;

    private float lifeTime;
    private Renderer rend;

    public GameObjectPool Pool { get; set; }

    private void OnEnable()
    {
        lifeTime = 0;
        rend = GetComponent<Renderer>();
    }

    void Update()
    {
        transform.Translate(Vector2.up * launchSpeed * Time.deltaTime);
        lifeTime += Time.deltaTime;

        if (lifeTime > maxLifeTime)
        {
            Pool.ReturnToPool(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((ignoredLayers & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            return;

        if (FX != null)
            FX.Play();
        Pool.ReturnToPool(this.gameObject);
    }

}
