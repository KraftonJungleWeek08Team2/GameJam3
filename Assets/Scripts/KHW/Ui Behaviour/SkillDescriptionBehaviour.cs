using System.Collections;
using TMPro;
using UnityEngine;

public class SkillDescriptionBehaviour : MonoBehaviour
{
    [SerializeField] private Canvas skillDescriptionCanvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float transparentTime = 2f;

    [SerializeField] private TextMeshProUGUI skillDescriptionText;

    void Awake()
    {
        skillDescriptionCanvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        skillDescriptionText = transform.Find("Skill Description").GetComponent<TextMeshProUGUI>();
        if (skillDescriptionCanvas == null || canvasGroup == null)
        {
            Debug.LogError("OneMoreUIBehaviour: Canvas or CanvasGroup component not found!");
            return;
        }

        canvasGroup.alpha = 0f; // 초기 상태: 완전히 투명
        skillDescriptionCanvas.enabled = false; // 초기 상태: 비활성화
    }

    public void Show(SlotInfo slotInfo, string description)
    {
        if (skillDescriptionCanvas == null || canvasGroup == null) return;
        //내용바꾸기
        skillDescriptionText.text = "[" + slotInfo.GetValue(0) + "," + slotInfo.GetValue(1) + "," + slotInfo.GetValue(2) +"]\n" + description;





        skillDescriptionCanvas.enabled = true;
        canvasGroup.alpha = 1f; // 완전히 불투명
        StopAllCoroutines(); // 이전 코루틴 중지





        
    }

    public void StartHide()
    {
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
        if (skillDescriptionCanvas != null)
        {
            skillDescriptionCanvas.enabled = false; // 캔버스 비활성화
        }
    }

}