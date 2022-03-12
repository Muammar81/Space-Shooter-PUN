using UnityEngine;

public class ScreenBoundaries : MonoBehaviour
{
    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;

    void Start()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        objectWidth = GetComponent<SpriteRenderer>().bounds.size.x / 2;
        objectHeight = GetComponent<SpriteRenderer>().bounds.size.y / 2;
    }

    void LateUpdate()
    {
        Vector2 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, -screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, -screenBounds.y + objectHeight, screenBounds.y - objectHeight);

        transform.position = viewPos;
    }
}

