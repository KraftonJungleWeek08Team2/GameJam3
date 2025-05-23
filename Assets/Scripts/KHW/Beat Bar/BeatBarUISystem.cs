using System.Collections;
using UnityEngine;

public class BeatBarUISystem : MonoBehaviour
{
    BeatBarSystem beatBarSystem;
    Canvas beatBarCanvas;


    [Header("UI References")]
    Transform accuracyPos; //판정 텍스트 생성 위치
    GameObject attackNoteObject; //공격해야하는 바 오브젝트.
    GameObject defaultBeatLineObject; //기본적으로 나오는 박자 바.
    GameObject perfectText;
    GameObject goodText;
    GameObject breakText;
    [SerializeField] ComboCountBehaviour comboCountBehaviour; //inspector.
    [SerializeField] OneMoreUIBehaviour oneMoreUIBehaviour;
    public SkillDescriptionBehaviour skillDescriptionBehaviour;
    public Transform AttackNoteTransforms;
    public Transform centralBarTransform;

    void Start()
    {   
        InitializeComponent(); //컴포넌트 초기화
        InitializeReferences(); //참조 초기화.
        SubscribeAction(); //비트바 생성시에 해당.
    }

    /// <summary> 컴포넌트 초기화 </summary>
    private void InitializeComponent()
    {
        beatBarSystem = GetComponent<BeatBarSystem>();
        beatBarCanvas = transform.parent.GetComponent<Canvas>();

        beatBarCanvas.enabled = false; //처음에는 숨김.
    }

    /// <summary> 참조 초기화 </summary>
    private void InitializeReferences()
    {
        skillDescriptionBehaviour = FindAnyObjectByType<SkillDescriptionBehaviour>();
        oneMoreUIBehaviour = FindAnyObjectByType<OneMoreUIBehaviour>();
        accuracyPos = transform.Find("Accuracy Position").transform;
        attackNoteObject = (GameObject)Resources.Load("KHW/Prefabs/NoteObject/AttackNoteObject");
        defaultBeatLineObject = (GameObject)Resources.Load("KHW/Prefabs/NoteObject/RestNoteObject");
        perfectText = (GameObject)Resources.Load("KHW/Prefabs/AccuracyText/PerfectTextObject");
        goodText = (GameObject)Resources.Load("KHW/Prefabs/AccuracyText/GoodTextObject");
        breakText = (GameObject)Resources.Load("KHW/Prefabs/AccuracyText/MissTextObject");
        centralBarTransform = transform.GetChild(1);
        AttackNoteTransforms = transform.GetChild(0);
    }

    /// <summary> 비트 바 캔버스 활성화 </summary>
    private void EnableBeatBar()
    {
        beatBarCanvas.enabled = true;
    }

    ///<summary> 비트 바 캔버스 비활성화 </summary>
    private void DisableBeatBar()
    {
        beatBarCanvas.enabled = false;
    }


    /// <summary> 액션 구독 -> 비트바 생성 액션에 자기 자신을 생성. </summary>
    private void SubscribeAction()
    {
        beatBarSystem.OnEnableBeatBarAction += EnableBeatBar;
        beatBarSystem.OnDisableBeatBarAction += DisableBeatBar;    
    }
    
    /// <summary> 액션 구독 해제 -> 일단 게임 꺼질 때 구독 해제. </summary>
    private void UnSubscribeAction()
    {
        beatBarSystem.OnEnableBeatBarAction -= EnableBeatBar;
        beatBarSystem.OnDisableBeatBarAction -= DisableBeatBar;    
    }

    private void OnDestroy()
    {
        UnSubscribeAction();
    }

    public void GenerateNoteUI(float timeDelay, string noteIndex)
    {
        StartCoroutine(GenerateNoteAfterTimeDelay(timeDelay, noteIndex));
    }

    private IEnumerator GenerateNoteAfterTimeDelay(float timeDelay, string noteIndex)
    {
        yield return new WaitForSeconds(timeDelay);
        GameObject newAttackNote = Instantiate(attackNoteObject, AttackNoteTransforms);
        newAttackNote.GetComponent<BeatBarBehaviour>().noteIndex = noteIndex;
    }
    public void NoteInputted(string noteIndex, AccuracyType accuracyType)
    {
        // attackNoteTransform의 모든 자식 오브젝트를 순회
        foreach (Transform child in AttackNoteTransforms)
        {
            BeatBarBehaviour note = child.GetComponent<BeatBarBehaviour>();
            if (note != null && note.noteIndex == noteIndex)
            {
                note.Disappear(accuracyType);
            }
        }
    }

    //TODO-KHW : 기본 박자선 필요하면 채우기.
    public void ShowBasicBeatLine()
    {
        Instantiate(defaultBeatLineObject, transform);
    }

    /// <summary> perfect 텍스트를 보여줍니다. </summary>
    public void ShowPerfectText()
    {
        Instantiate(perfectText, accuracyPos);
    }

    /// <summary> good 텍스트를 보여줍니다. </summary>
    public void ShowGoodText()
    {
        Instantiate(goodText, accuracyPos);
    }

    /// <summary> break 텍스트를 보여줍니다. </summary>
    public void ShowBreakText()
    {
        Instantiate(breakText, accuracyPos);
    }

    public void ShowComboCount(int currentCombo)
    {
        comboCountBehaviour.UpdateComboCount(currentCombo);
    }

    public void ShowOneMoreUI()
    {
        oneMoreUIBehaviour.Show();
    }


    public void ShowSkillDescriptionUI(string description)
    {
        skillDescriptionBehaviour.Show(beatBarSystem.currentSlotInfo, description);
    }

    public void HideSkillDescriptionUI()
    {
        skillDescriptionBehaviour.StartHide();
    }
}
