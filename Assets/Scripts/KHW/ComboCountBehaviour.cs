using TMPro;
using UnityEngine;

public class ComboCountBehaviour : MonoBehaviour
{
    [SerializeField] private float duration = 0.3f; // 애니메이션 지속 시간 (초)
    [SerializeField] private float moveDistance = 50f; // 위로 이동할 거리 (픽셀)
    [SerializeField] private int comboCountThreshold = 3;
    private CanvasGroup canvasGroup;
    private float elapsedTime = 0f;
    //private Vector3 initialPosition;
    private TextMeshProUGUI comboCountText;
    private bool isShowing;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        comboCountText = GetComponent<TextMeshProUGUI>();
        if (canvasGroup == null || comboCountText == null)
        {
            Debug.LogError("ComboCountBehaviour: CanvasGroup or TextMeshProUGUI component not found!");
            return;
        }

        //initialPosition = transform.localPosition; // 초기 위치 저장
        HideComboCount(); // 초기 상태: 숨김
    }

    void Update()
    {
        if (!isShowing) return; // 표시 중이 아니면 업데이트 중지

        elapsedTime += Time.deltaTime;
        float t = elapsedTime / duration; // 0에서 1로 진행률 계산

        // 알파 값: 1에서 0으로 페이드 아웃
        canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

        // 위치: 위로 이동
        //transform.localPosition = Vector3.Lerp(initialPosition, initialPosition + Vector3.up * moveDistance, t);

        // 애니메이션 종료 시 숨김
        if (t >= 1f)
        {
            HideComboCount();
        }
    }

    public void UpdateComboCount(int count)
    {
        comboCountText.text = count + " Combo";

        if (count < comboCountThreshold)
        {
            HideComboCount();
        }
        else
        {
            ShowComboCount();
        }
    }

    void HideComboCount()
    {
        isShowing = false;
        canvasGroup.alpha = 0f; // 즉시 숨김
    }

    void ShowComboCount()
    {
        isShowing = true;
        canvasGroup.alpha = 1f; // 즉시 표시
        elapsedTime = 0f; // 애니메이션 시간 리셋
        //transform.localPosition = initialPosition; // 위치 리셋
    }
}