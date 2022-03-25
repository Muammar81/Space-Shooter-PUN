using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField] private float launchSpeed = 5f;
    private Camera cam;
    private float offset = 300f;

    private void Awake() => cam = Camera.main;
    private void Update()
    {
        transform.Translate(Vector2.right * launchSpeed * Time.deltaTime);

        if (!IsInScreen(transform))
            AstroidSpawner.ReturnToPool(gameObject);
    }

    private bool IsInScreen(Transform t)
    {
        var screenPos = cam.WorldToScreenPoint(t.position);

        return screenPos.x > 0f &&
                screenPos.x < Screen.width + offset &&
                screenPos.y > 0f &&
                screenPos.y < Screen.height;
    }
}
