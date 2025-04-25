using System;
using System.Collections.Generic;
using UnityEngine;

public class BeatBarSystem : MonoBehaviour
{
    [SerializeField] public int BaseBeat {get; private set;} //트리거 될 때의 비트. 조금 더해주던지 해야할 듯?
    [SerializeField] public int EndBeat {get; private set;} //비트바가 사라지는 비트.
    [SerializeField] public int CurrentMusicBeat {get; private set;} // 현재 musicManager의 BeatCount.
    //[SerializeField] private MusicManager musicManager; // music Manager 가져와서 쓰기.
    [SerializeField] private GameObject attackNoteObject; //노트 프리팹.
    [SerializeField] private GameObject restNoteObject;
    [SerializeField] private Transform accuracyPos;
    //[SerializeField] private Transform ComboCountPos;
    [SerializeField] private GameObject perfectText; // 퍼펙트 판정의 텍스트를 가진 gameobject.
    [SerializeField] private GameObject goodText; //굿 판정의 텍스트를 가진 gameobject.
    [SerializeField] private GameObject breakText; //실패 판정의 텍스트를 가진 gameobject.
    [SerializeField] private SlotInfo currentSlotInfo; //받아올 슬롯의 정보를 가진 struct.
    [SerializeField] private List<int> attackBeatCounts; 
    [SerializeField] private bool currentBeatInputted; // 현재 비트에 입력이 있었는가?
    [SerializeField] private Canvas beatBarCanvas;
    [SerializeField] private int noteMargin = 2; // 노트 출현 후 가운데까지 오는데 걸리는 비트
    [SerializeField] private bool isFullCombo = true;
    [SerializeField] private int currentComboCount = 0;
    [SerializeField] ComboCountBehaviour comboCountBehaviour; //inspector.
    [SerializeField] OneMoreUIBehaviour oneMoreUIBehaviour;

    public OneMoreUIBehaviour OneMoreUIBehaviour => oneMoreUIBehaviour; // 외부에서 접근할 수 있도록 프로퍼티 추가

    public Action<bool> OnEndRhythmEvent;       // 리듬 공격 전체가 끝날 때의 액션, 풀콤 여부를 변수로 넘겨줌
    public Action<AccuracyType> OnAttackEvent;  // 각 리듬 공격의 판정을 enmu으로 넘겨줌
    public Action OnEnableBeatBarAction;
    public Action OnDisableBeatBarAction;

    BeatBarUISystem beatBarUISystem;
    BeatInputChecker beatInputChecker;
    BeatBarNoteSpawner beatBarNoteSpawner;
    BeatBarResultSystem beatBarResultSystem;


    void Start()
    {
        InitializeBeatBar();
        //InitializePropertiesSelf();
    }

    /// <summary>
    /// 컴포넌트를 참조.
    /// </summary>
    void InitializeBeatBar()
    {
        //Other Components of BeatBar.
        beatBarUISystem = GetComponent<BeatBarUISystem>();
        beatInputChecker = GetComponent<BeatInputChecker>();
        beatBarNoteSpawner = GetComponent<BeatBarNoteSpawner>();
        beatBarResultSystem = GetComponent<BeatBarResultSystem>();

        //Action Subscribe.
        // MusicManager.Instance.OnNextBeatAction += UpdateCurrentBeat;
        // MusicManager.Instance.OnBeatAction += GenerateNewNote;
    }


    

    /// <summary>
    /// Beat Bar를 활성화합니다.
    /// </summary>
    /// <param name="slotInfo"></param>
    public void ActivateBeatBar(SlotInfo slotInfo)
    {
        currentSlotInfo = slotInfo; 
        OnEnableBeatBarAction?.Invoke(); //비트바 활성화시 수행할 액션 전부 트리거.
    }

    public void DisableBeatBar()
    {
        //INPUT 구독 제거
        Managers.InputManager.OnRhythmAttackEvent -= Attack;
        OnDisableBeatBarAction?.Invoke();

    }

    private void OnBeat()
    {

    }

    // public void ShowBeatBar(SlotInfo slotInfo) //비트바 활동 시작.
    // {
    //     beatBarCanvas.enabled = true;
    //     InitializePropertiesOther(slotInfo);
    //     baseBeat = MusicManager.Instance.currentBeat + noteMargin + 2;

    //     int totalBeatOfSlotInfo = 1; // 초기값 1 제거
    //     for (int i = 0; i < currentSlotInfo.SlotCount; i++)
    //     {
    //         totalBeatOfSlotInfo += currentSlotInfo.GetValue(i);
    //     }
    //     endBeat = baseBeat + totalBeatOfSlotInfo;
    //     isFullCombo = true;
    //     Managers.InputManager.OnRhythmAttackEvent += Attack;
    // }

    public void HideBeatBar() //비트바 활종.
    {

    }

    void InitializePropertiesSelf()
    {
        musicManager = MusicManager.Instance;
        accuracyPos = transform.Find("Accuracy Position").transform;
        attackNoteObject = (GameObject)Resources.Load("KHW/Prefabs/NoteObject/AttackNoteObject");
        restNoteObject = (GameObject)Resources.Load("KHW/Prefabs/NoteObject/RestNoteObject");
        perfectText = (GameObject)Resources.Load("KHW/Prefabs/AccuracyText/PerfectTextObject");
        goodText = (GameObject)Resources.Load("KHW/Prefabs/AccuracyText/GoodTextObject");
        breakText = (GameObject)Resources.Load("KHW/Prefabs/AccuracyText/MissTextObject");
        musicManager.OnNextBeatAction += UpdateCurrentBeat;
        musicManager.OnBeatAction += GenerateNewNote;
        oneMoreUIBehaviour = FindAnyObjectByType<OneMoreUIBehaviour>();

        beatBarCanvas = transform.parent.GetComponent<Canvas>();
        beatBarCanvas.enabled = false;
    }

/// <summary>
/// slotInfo로부터 공격비트 숫자를 얻어오는 함수.. 노트가 가운데 도달하기까지는 margin bit.
/// </summary>
    bool GetIsAttackBeatCount(int currentBeat)
    {
        bool isAttackBeat = false;
        int currentBeatBase = BaseBeat;

        for(int i = 0; i < currentSlotInfo.SlotCount; i++)
        {
            if(currentBeat >= currentBeatBase && currentBeat <= currentBeatBase + currentSlotInfo.GetValue(i) - 1) //사이값인가?
            {
                return true;
            }

            currentBeatBase += currentSlotInfo.GetValue(i) + 1;
        }

        return isAttackBeat;
    }

/// <summary>
/// 
/// </summary>
/// <param name="currentBeat"></param>
    private void GenerateNewNote(int currentBeat)
    {
        if(GetIsAttackBeatCount(currentBeat + noteMargin))
        {
            Instantiate(attackNoteObject, transform);
            Debug.Log("beatbar -> Attack Beat : " + currentBeat);
        }
        else
        {
            Instantiate(restNoteObject, transform);
            //Debug.Log("Rest Beat : " + currentBeat);
        }

    }

    private void UpdateCurrentBeat(int currentBeat) //currentBeat는 현재 musicManager의 currentBeat. OnNextBeat에서 Update.
    {
        if(GetIsAttackBeatCount(currentBeat - 1) && !currentBeatInputted) //이전 값이 공격인데, 공격을 안함?
        {

            Instantiate(breakText, accuracyPos); //break이니라.
            isFullCombo = false;

            if(currentBeat - 1 == EndBeat) //마지막 비트임?
            {
                Debug.Log("Log : 마지막 비트 놓침");
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }
        }

        currentBeatInputted = false; //입력 리셋
        CurrentMusicBeat = currentBeat;
        
    }


    void Update()
    {
              

    }

/// <summary>
/// Triggered By PlayerInput Component.
/// </summary>
    void Attack() 
    {



    void OnDestroy()
    {
        musicManager.OnNextBeatAction -= UpdateCurrentBeat;
        musicManager.OnBeatAction -= GenerateNewNote;
        
        //TODO : 더 정확한 타이밍에 제거해주기 임시방편
        Managers.InputManager.OnRhythmAttackEvent -= Attack;
    }
}
