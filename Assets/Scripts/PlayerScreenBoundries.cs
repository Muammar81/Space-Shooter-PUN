using UnityEngine;

public class PlayerScreenBoundries : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSprite;

    private Vector2 screenBounds;
    private float playerWidth;
    private float playerHeight;

    private void Start()
    {
        Camera cam = Camera.main;
        screenBounds = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        playerWidth = playerSprite.bounds.size.x/ 2;
        playerHeight = playerSprite.bounds.size.y/ 2;
    }
    private void LateUpdate()
    {
        Vector2 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + playerWidth, -screenBounds.x - playerWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y + playerHeight, screenBounds.y - playerHeight);

        transform.position = viewPos;
    }

}
