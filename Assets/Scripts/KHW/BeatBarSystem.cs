using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class Note
{
    [SerializeField] private int beat; // 기준 비트
    [SerializeField] private float offsetBeat; // 기준 비트와의 오프셋 (0~1 비트)
    [SerializeField] public bool isLast; // 마지막 노트 여부

    public int Beat => beat;
    public float OffsetBeat => offsetBeat;
    public bool IsLast => isLast;

    public Note(int baseBeat, float offsetBeat, bool isLast = false)
    {
        this.beat = baseBeat;
        this.offsetBeat = Mathf.Clamp(offsetBeat, 0f, 1f); // 0~1 비트 제한
        this.isLast = isLast;
    }

    // Unity 직렬화를 위한 기본 생성자
    public Note()
    {
        beat = 0;
        offsetBeat = 0f;
        isLast = false;
    }
}

public class BeatBarSystem : MonoBehaviour
{


    [Header("Actions")]
    public Action OnEnableBeatBarAction;
    public Action OnDisableBeatBarAction;

    [Header("Components")]
    BeatBarUISystem beatBarUISystem;
    BeatInputChecker beatInputChecker;
    //BeatBarNoteSpawner beatBarNoteSpawner;
    //BeatBarResultSystem beatBarResultSystem;
    PatternManager patternManager;

    [Header("Beat Bar Informations")]
    public SlotInfo currentSlotInfo {get; private set;}
    public List<Note> currentNotes {get; private set;}
    private float noteInterval;
    private int currentIndexOfNote;
    public Note currentNote {get; private set;}
    [SerializeField] public int BaseBeat {get; private set;} //트리거 될 때의 비트. 조금 더해주던지 해야할 듯?
    [SerializeField] public int CurrentMusicBeat {get; private set;} // 현재 musicManager의 BeatCount
    [SerializeField] private int noteMargin = 3; // 노트 출현 후 가운데까지 오는데 걸리는 비트

    void Start()
    {
        InitializeBeatBar();
        SubscribeAction();
    }

    /// <summary>
    /// 컴포넌트를 참조.
    /// </summary>
    void InitializeBeatBar()
    {
        //Other Components of BeatBar.
        beatBarUISystem = GetComponent<BeatBarUISystem>();
        beatInputChecker = GetComponent<BeatInputChecker>();
        //beatBarNoteSpawner = GetComponent<BeatBarNoteSpawner>();
        //beatBarResultSystem = GetComponent<BeatBarResultSystem>();
        patternManager = GetComponent<PatternManager>();
    }

    /// <summary>
    /// Beat Bar를 활성화합니다.
    /// </summary>
    /// <param name="slotInfo"></param>
    public void ActivateBeatBar(SlotInfo slotInfo)
    {
        currentSlotInfo = slotInfo; 
        CurrentMusicBeat = MusicManager.Instance.currentBeat;
        noteInterval = MusicManager.Instance.beatInterval;
        GenerateNoteList();

        OnEnableBeatBarAction?.Invoke(); //비트바 활성화시 수행할 액션 전부 트리거.

    }

    /// <summary> slot info 를 기반으로 모든 노트의 리스트를 얻어냅니다. </summary>
    void GenerateNoteList()
    {
        currentNotes = patternManager.GetNoteListBySlotInfo(MusicManager.Instance.currentBeat + noteMargin, currentSlotInfo);
        currentIndexOfNote = 0;
        currentNote = currentNotes[0];
    }

    void GenerateNewNote(int currentBeat)
    {
        //beatBarUISystem.ShowBasicBeatLine();
        CurrentMusicBeat = currentBeat;
        if(currentNote == null || currentNotes == null) return;

        int targetBeat = currentBeat + noteMargin;
        
        List<Note> notes = currentNotes.Where(n => n.Beat == targetBeat).ToList();

        Debug.Log($"GenerateNewNote: currentBeat={currentBeat}, targetBeat={targetBeat}, notes.Count={notes.Count}");
        if (notes.Count > 0)
        {
            foreach (var note in notes)
            {
                float timeDelay = note.OffsetBeat * noteInterval;
                beatBarUISystem.GenerateNoteUI(timeDelay);
            }
        }
        else
        {
            Debug.Log($"No notes for targetBeat={targetBeat}");
        }
    }
    public void ChangeCurrentNote()
    {
        currentIndexOfNote++;

        if(currentIndexOfNote < currentNotes.Count) //안끝남.
        {
            currentNote = currentNotes[currentIndexOfNote]; //다음 노트로 현재 노트를 변경.
        }
        else
        {

        }
    }

    /// <summary> Beat Bar 비활성화 </summary>
    public void DisableBeatBar()
    {
        OnDisableBeatBarAction?.Invoke();

        UnsubscribeAction();
    }
    
    // <summary> 액션을 구독합니다. </summary>
    void SubscribeAction()
    {
        MusicManager.Instance.OnBeatAction += GenerateNewNote;
        
    }

    /// <summary> 액션을 구독해제합니다. </summary>
    void UnsubscribeAction()
    {
        MusicManager.Instance.OnBeatAction -= GenerateNewNote;
    }

    void OnDestroy()
    {
        UnsubscribeAction(); //구독 해제.
    }
}