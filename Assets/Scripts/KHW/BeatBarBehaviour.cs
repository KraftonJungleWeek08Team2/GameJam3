using UnityEngine;
using UnityEngine.UI;

public class BeatBarBehaviour : MonoBehaviour
{
    [SerializeField] private int beatMargin;
    private float smallingTime; // Time to shrink width to 0
    private RectTransform rectTransform;
    private float initialWidth;
    private float elapsedTime;
    void Start()
    {
        smallingTime = MusicManager.Instance.noteInterval * beatMargin;
        rectTransform = GetComponent<RectTransform>();
        initialWidth = rectTransform.sizeDelta.x; // Store initial width
        elapsedTime = 0f;
    }

    void Update()
    {
        if (elapsedTime < smallingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / smallingTime; // Interpolation factor (0 to 1)
            float newWidth = Mathf.Lerp(initialWidth, 0f, t); // Interpolate width
            rectTransform.sizeDelta = new Vector2(newWidth, rectTransform.sizeDelta.y); // Update width
        }
    }
}