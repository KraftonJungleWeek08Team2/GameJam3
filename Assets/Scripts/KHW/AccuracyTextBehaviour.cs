using UnityEngine;

public class AccuracyTextBehaviour : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f; // 애니메이션 지속 시간 (초)
    [SerializeField] private float moveDistance = 50f; // 위로 이동할 거리 (픽셀)
    private CanvasGroup canvasGroup;
    private float elapsedTime = 0f;
    private Vector3 initialPosition;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("AccuracyTextBehaviour: CanvasGroup component not found!");
            return;
        }

        initialPosition = transform.localPosition; // 초기 위치 저장
        Destroy(gameObject, duration); // 지정된 시간 후 객체 파괴
    }

    void Update()
    {
        if (canvasGroup == null) return;

        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration; // 0에서 1로 진행률 계산

        // 알파 값: 1에서 0으로 페이드 아웃
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

        // 위치: 위로 이동
        transform.localPosition = initialPosition + new Vector3(0f, moveDistance * t, 0f);
    }
}