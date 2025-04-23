using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;

/// <summary>
/// SetActive? Instantiate?
/// </summary>
public class BeatBarPanelBehaviour : MonoBehaviour
{
    [SerializeField] private int baseBeat; //트리거 될 때의 비트. 조금 더해주던지 해야할 듯?
    [SerializeField] private int endBeat; //비트바가 사라지는 비트.
    [SerializeField] private int currentMusicBeat; // 현재 musicManager의 BeatCount.
    [SerializeField] private MusicManager musicManager; // music Manager 가져와서 쓰기.
    [SerializeField] private GameObject attackNoteObject; //노트 프리팹.
    [SerializeField] private GameObject restNoteObject;
    [SerializeField] private Transform accuracyPos;
    [SerializeField] private GameObject perfectText; // 퍼펙트 판정의 텍스트를 가진 gameobject.
    [SerializeField] private GameObject goodText; //굿 판정의 텍스트를 가진 gameobject.
    [SerializeField] private GameObject breakText; //실패 판정의 텍스트를 가진 gameobject.
    [SerializeField] private SlotInfo currentSlotInfo; //받아올 슬롯의 정보를 가진 struct.
    [SerializeField] private List<int> attackBeatCounts; 
    [SerializeField] private bool currentBeatInputted; // 현재 비트에 입력이 있었는가?
    [SerializeField] private Canvas beatBarCanvas;
    [SerializeField] private int noteMargin = 2; // 노트 출현 후 가운데까지 오는데 걸리는 비트
    [SerializeField] private bool isFullCombo = true;

    void Start()
    {
        InitializePropertiesSelf();
    }

    public void ShowBeatBar(SlotInfo slotInfo) //비트바 활동 시작.
    {
        beatBarCanvas.enabled = true;
        InitializePropertiesOther(slotInfo);
        baseBeat = MusicManager.Instance.currentBeat + noteMargin; //노트 도달 4비트. 여유 2비트.



        int totalBeatOfSlotInfo = 1;
        for(int i = 0 ; i < currentSlotInfo.SlotCount; i++)
        {
            totalBeatOfSlotInfo += currentSlotInfo.GetValue(i);
        }

        endBeat = baseBeat + totalBeatOfSlotInfo;
        
    }

    public void HideBeatBar() //비트바 활종.
    {
        //INPUT 구독 제거?
        Managers.InputManager.OnRhythmAttackEvent -= Attack;
        // 끝나는 시점에서 호출
        Managers.TurnManager.IsFullCombo = isFullCombo;
        Managers.TurnManager.EndAttackState();
        beatBarCanvas.enabled = false;


   
    }

    public void InitializePropertiesOther(SlotInfo slotInfo)
    {
        currentSlotInfo = slotInfo; //슬롯정보 받아오기.
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

        Managers.InputManager.OnRhythmAttackEvent += Attack;

        beatBarCanvas = transform.parent.GetComponent<Canvas>();
        beatBarCanvas.enabled = false;

        
    }

/// <summary>
/// slotInfo로부터 공격비트 숫자를 얻어오는 함수.. 노트가 가운데 도달하기까지는 margin bit.
/// </summary>
    bool GetIsAttackBeatCount(int currentBeat)
    {
        bool isAttackBeat = false;
        int currentBeatBase = baseBeat;

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
            Debug.Log("Attack Beat : " + currentBeat);
        }
        else
        {
            Instantiate(restNoteObject, transform);
            Debug.Log("Rest Beat : " + currentBeat);
        }

    }

    private void UpdateCurrentBeat(int currentBeat) //currentBeat는 현재 musicManager의 currentBeat. OnNextBeat에서 Update.
    {
        if(GetIsAttackBeatCount(currentBeat - 1) && !currentBeatInputted) //이전 값이 공격인데, 공격을 안함?
        {

            Instantiate(breakText, accuracyPos); //break이니라.
            isFullCombo = false;

            if(currentBeat - 1 == endBeat) //마지막 비트임?
            {
                HideBeatBar();
            }
        }

        currentBeatInputted = false; //입력 리셋
        currentMusicBeat = currentBeat;
    }


    void Update()
    {
              

    }

/// <summary>
/// Triggered By PlayerInput Component.
/// </summary>
    void Attack() 
    {
        Debug.Log("현재 비트 : " + currentMusicBeat);
        
        float accuracy = 1 - Mathf.Abs(musicManager.GetTimingOffset() / (musicManager.noteInterval) / 2);

        Debug.Log("정확도 : " + accuracy);

        if(accuracy > 0.7 && GetIsAttackBeatCount(currentMusicBeat - noteMargin) && !currentBeatInputted) //Perfect Attack.
        {
            //Managers.TurnManager.CurrentEnemy.TakeDamage(2);            
            if(currentMusicBeat == endBeat) //마지막 비트에 입력 성공.
            {
                HideBeatBar();
            }
            currentBeatInputted = true;
            Instantiate(perfectText, accuracyPos);
        } 
        else if(accuracy > 0.4 && GetIsAttackBeatCount(currentMusicBeat - noteMargin) && !currentBeatInputted)
        {   
            //Managers.TurnManager.CurrentEnemy.TakeDamage(1);
            if(currentMusicBeat == endBeat) //마지막 비트에 입력 성공.
            {
                HideBeatBar();
            }
            currentBeatInputted = true;
            Instantiate(goodText, accuracyPos);
        }
        else if(!GetIsAttackBeatCount(currentMusicBeat - noteMargin) && !currentBeatInputted) //아닌데 누름.
        {
            currentBeatInputted = true;
            isFullCombo = false;
            Instantiate(breakText, accuracyPos);
        }
        else if(currentBeatInputted) //중복 입력
        {   
            isFullCombo = false;
            //Instantiate(breakText, accuracyPos);
        }
    }

    void OnDestroy()
    {
        Managers.InputManager.OnRhythmAttackEvent -= Attack;
    }
}
