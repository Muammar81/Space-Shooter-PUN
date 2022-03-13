using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] [Tooltip("SFX particle")] ParticleSystem FX;
    [SerializeField] [Tooltip("Typically, player's layer")] private LayerMask hitLayers;
    [SerializeField] private float minSpeed = 0.01f;
    [SerializeField] private float maxSpeed = 3f;

    private Camera cam;
    private float launchSpeed;

    private void OnEnable()
    {
        cam = Camera.main;
        launchSpeed = Random.Range(minSpeed, maxSpeed);
        FX.playOnAwake = false;
    }

    void Update()
    {
        transform.Translate(Vector2.up * launchSpeed * Time.deltaTime);

        if (!IsInScreen(transform))
            AstroidSpawner.ReturnToPool(this.gameObject);
    }


    private bool IsInScreen(Transform t)
    {
        var screenPos = cam.WorldToScreenPoint(t.position);

        return screenPos.x > 0f &&
               screenPos.x < Screen.width &&
               screenPos.y > 0f &&
               screenPos.y < Screen.height;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if ((hitLayers & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            print("player hit with astroid");
            FX?.Play();
            PlayerWeapon.ReturnToPool(this.gameObject);
        }
    }

}
