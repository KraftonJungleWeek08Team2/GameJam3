using System.Collections;
using TMPro;
using UnityEngine;

public class SkillDescriptionBehaviour : MonoBehaviour
{
    [SerializeField] private Canvas skillDescriptionCanvas;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI skillDescriptionText;

    [Header("Animiation Properties")]
    [SerializeField] private Animator animator;
    int beatAnimId;
    int failAnimId;
    int successAnimId;
    int resetAnimId;

    void Awake()
    {
        skillDescriptionCanvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        animator = GetComponent<Animator>();

        InitializeAnimationID();

        skillDescriptionText = transform.Find("Skill Description").GetComponent<TextMeshProUGUI>();
        if (skillDescriptionCanvas == null || canvasGroup == null)
        {
            Debug.LogError("OneMoreUIBehaviour: Canvas or CanvasGroup component not found!");
            return;
        }

        canvasGroup.alpha = 0f; // 초기 상태: 완전히 투명
        skillDescriptionCanvas.enabled = false; // 초기 상태: 비활성화
    }

    void Start()
    {
        MusicManager.Instance.OnBeatAction -= BeatUI;
        MusicManager.Instance.OnBeatAction += BeatUI;
    }

    private void InitializeAnimationID()
    {
        beatAnimId = Animator.StringToHash("Beat");
        failAnimId = Animator.StringToHash("Fail");
        successAnimId = Animator.StringToHash("Success");
        resetAnimId = Animator.StringToHash("Reset");
    }

    public void Show(SlotInfo slotInfo, string description)
    {
        if (skillDescriptionCanvas == null || canvasGroup == null) return;

        if(CombinationChecker.Check(slotInfo) == null) return;

        //내용바꾸기
        skillDescriptionText.text = "[" + slotInfo.GetValue(0) + "," + slotInfo.GetValue(1) + "," + slotInfo.GetValue(2) +"]\n" + description;

        canvasGroup.alpha = 1;
        skillDescriptionCanvas.enabled = true;

        animator.SetBool(successAnimId, false);
        animator.SetBool(failAnimId, false);
        animator.SetTrigger(resetAnimId);
    }

    /// <summary> 실패시 실행. </summary>
    public void FastHide()
    {
        animator.SetBool(failAnimId, true);
        StartCoroutine(OffCoroutine());
    }

    /// <summary> 성공시 실행. </summary>
    public void StartHide()
    {
        Debug.Log("Start hide");
        animator.SetBool(successAnimId, true);
        StartCoroutine(OffCoroutine());
    }

    IEnumerator OffCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        animator.SetBool(failAnimId, false);
        animator.SetBool(successAnimId, false);
    }

    /// <summary> 음악 비트시 실행 </summary>
    /// <param name="currentBeat"></param>
    private void BeatUI(int currentBeat)
    {
        animator.SetTrigger(beatAnimId);
    }

    private void OnDestroy()
    {
        MusicManager.Instance.OnBeatAction -= BeatUI;
    }
}