using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] [Tooltip("SFX particle")] ParticleSystem FX;
    [SerializeField] [Tooltip("Typically, player's layer")] private LayerMask hitLayers;
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private float maxSpeed = 3f;

    private Camera cam;
    private float launchSpeed;
    [SerializeField] private float offScreenMargin;

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
        float offset = 300;
        var screenPos = cam.WorldToScreenPoint(t.position );

        return screenPos.x > 0f &&
               screenPos.x  < Screen.width +offset &&
               screenPos.y > 0f &&
               screenPos.y  < Screen.height;
    }
}
