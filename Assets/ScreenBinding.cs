using UnityEngine;

public class ScreenBinding : MonoBehaviour
{
    private Camera cam;
    private Vector2 screenBounds;
    private Vector2 playerBounds;

    void Start()
    {
        cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));

        var rend = transform.GetComponent<SpriteRenderer>();
        playerBounds = new Vector2(rend.bounds.extents.x, rend.bounds.extents.y);
    }

    void LateUpdate()
    {
        Vector2 boundPosition = transform.position;
        boundPosition.x = Mathf.Clamp(-boundPosition.x, screenBounds.x + playerBounds.x, screenBounds.x - playerBounds.x);
        boundPosition.y = Mathf.Clamp(-boundPosition.y, screenBounds.y + playerBounds.y, screenBounds.y - playerBounds.y);
        transform.position = boundPosition;
    }
}
