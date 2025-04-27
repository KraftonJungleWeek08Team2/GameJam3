using System;
using System.Collections;
using UnityEngine;

public class OneMoreUIBehaviour : MonoBehaviour
{
    [SerializeField] private Canvas oneMoreUICanvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float transparentTime = 2f;

    void Awake()
    {
        oneMoreUICanvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (oneMoreUICanvas == null || canvasGroup == null)
        {
            Debug.LogError("OneMoreUIBehaviour: Canvas or CanvasGroup component not found!");
            return;
        }

        canvasGroup.alpha = 0f; // 초기 상태: 완전히 투명
        oneMoreUICanvas.enabled = false; // 초기 상태: 비활성화
    }

    public void Show()
    {
        if (oneMoreUICanvas == null || canvasGroup == null) return;

        oneMoreUICanvas.enabled = true;
        canvasGroup.alpha = 1f; // 완전히 불투명
        StopAllCoroutines(); // 이전 코루틴 중지
        StartCoroutine(Hide());
    }

    private IEnumerator Hide()
    {
        float elapsed = 0f;
        while (elapsed < transparentTime)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / transparentTime); // 페이드 아웃
            yield return null;
        }
        canvasGroup.alpha = 0f; // 최종적으로 완전히 투명
        if (oneMoreUICanvas != null)
        {
            oneMoreUICanvas.enabled = false; // 캔버스 비활성화
        }
    }

}