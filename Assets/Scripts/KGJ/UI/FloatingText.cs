using UnityEngine;

public class FloatingText : MonoBehaviour
{
    public float textMoveRange = 30f; // 움직임의 범위 (UI 단위에서는 일반적으로 픽셀 단위이므로 값 증가)
    public float textMoveSpeed = 1.0f; // 움직임의 속도

    private RectTransform rectTransform;
    private Vector2 startAnchoredPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startAnchoredPosition = rectTransform.anchoredPosition; // 초기 앵커 위치 저장
    }

    void Update()
    {
        float newY = startAnchoredPosition.y + Mathf.Sin(Time.time * textMoveSpeed) * textMoveRange;
        rectTransform.anchoredPosition = new Vector2(startAnchoredPosition.x, newY);
    }
}
