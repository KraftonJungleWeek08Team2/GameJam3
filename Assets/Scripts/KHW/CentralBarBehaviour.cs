using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Image 컴포넌트 사용

public class CentralBarBehaviour : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup; // 투명도 조절용
    [SerializeField] private Image centralBarImage; // 색상 조절용
    [SerializeField] private float glowDuration = 0.2f; // 깜빡임 지속 시간 (초)
    [SerializeField] private Color normalColor = Color.white; // 기본 색상
    [SerializeField] private Color glowColor = new Color(1f, 1f, 0.5f); // 깜빡임 색상 (밝은 노랑)
    [SerializeField] private float maxScale = 1.2f; // 최대 스케일 (1.2배)
    private bool isGlowing = false; // 중복 코루틴 방지
    private Vector3 originalScale; // 원래 스케일 저장

    void Start()
    {
        // 컴포넌트 초기화
        canvasGroup = GetComponent<CanvasGroup>();
        centralBarImage = GetComponent<Image>();
        originalScale = transform.localScale; // 원래 스케일 저장

        if (canvasGroup == null)
        {
            Debug.LogError("CentralBarBehaviour: CanvasGroup component not found!");
        }
        if (centralBarImage == null)
        {
            Debug.LogError("CentralBarBehaviour: Image component not found!");
        }

        MusicManager.Instance.OnBeatAction += GlowCentralBar;
    }

        void OnDestroy()
        {
            // 이벤트 구독 해제
            MusicManager.Instance.OnBeatAction -= GlowCentralBar;
        }

        void GlowCentralBar(int currentBeat)
        {
            if (!isGlowing) // 중복 실행 방지
            {
                StartCoroutine(Glow());
            }
        }

    private IEnumerator Glow()
    {
        isGlowing = true;

        if (canvasGroup != null && centralBarImage != null)
        {
            float halfDuration = glowDuration / 2f;
            float elapsed = 0f;
            Vector3 glowScale = originalScale * maxScale; // 최대 스케일 계산

            // 1. 기본 → 밝은 상태 (alpha, color, scale 변화)
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                canvasGroup.alpha = Mathf.Lerp(1f, 0.8f, t); // 약간 투명해짐
                centralBarImage.color = Color.Lerp(normalColor, glowColor, t);
                transform.localScale = Vector3.Lerp(originalScale, glowScale, t); // 스케일 증가
                yield return null;
            }

            // 2. 밝은 상태 → 기본 상태
            elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / halfDuration;
                canvasGroup.alpha = Mathf.Lerp(0.8f, 1f, t); // 다시 불투명
                centralBarImage.color = Color.Lerp(glowColor, normalColor, t);
                transform.localScale = Vector3.Lerp(glowScale, originalScale, t); // 스케일 복귀
                yield return null;
            }

            // 최종 상태 보장
            canvasGroup.alpha = 1f;
            centralBarImage.color = normalColor;
            transform.localScale = originalScale;
        }

        isGlowing = false;
    }
}