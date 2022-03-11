using UnityEngine;
using UnityEngine.UI;

public class SideScroller : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 1f)]
    private float scrollSpeed = 0.1f;
    private RawImage img;

    private void Awake() => img = GetComponent<RawImage>();
    void Update() => img.uvRect = new Rect(img.uvRect.position + Vector2.right * scrollSpeed * Time.deltaTime, img.uvRect.size);
}
