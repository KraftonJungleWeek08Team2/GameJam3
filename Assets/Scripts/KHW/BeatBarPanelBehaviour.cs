using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// SetActive? Instantiate?
/// </summary>
public class BeatBarPanelBehaviour : MonoBehaviour
{
    [SerializeField] private int baseBeat; //트리거 될 때의 비트. 조금 더해주던지 해야할 듯?
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
    void Start()
    {
        InitializePropertiesSelf();
        currentSlotInfo = new SlotInfo(2);
        currentSlotInfo.SetValue(0, 5); //임시
        currentSlotInfo.SetValue(1, 3);
    }

    public void InitializePropertiesOther(SlotInfo slotInfo)
    {
        currentSlotInfo = slotInfo; //슬롯정보 받아오기.
        //GetAttackBeatCounts(); //바로 계산.
    }

    void InitializePropertiesSelf()
    {
        musicManager = MusicManager.Instance;
        accuracyPos = transform.Find("Accuracy Position").transform;
        attackNoteObject = (GameObject)Resources.Load("KHW/Prefabs/AttackNoteObject");
        restNoteObject = (GameObject)Resources.Load("KHW/Prefabs/RestNoteObject");
        perfectText = (GameObject)Resources.Load("KHW/Prefabs/PerfectTextObject");
        goodText = (GameObject)Resources.Load("KHW/Prefabs/GoodTextObject");
        breakText = (GameObject)Resources.Load("KHW/Prefabs/BreakTextObject");
        musicManager.OnNextBeatAction += UpdateCurrentBeat;
        musicManager.OnBeatAction += GenerateNewNote;
        baseBeat = MusicManager.Instance.currentBeat + 6; //노트 도달 4비트. 여유 2비트.
        
    }

/// <summary>
/// slotInfo로부터 공격비트 숫자를 얻어오는 함수.. 노트가 가운데 도달하기까지는 4비트.
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
        if(GetIsAttackBeatCount(currentBeat))
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
        currentBeatInputted = false; //입력 리셋
        currentMusicBeat = currentBeat;
    }


    void Update()
    {
              

    }

/// <summary>
/// Triggered By PlayerInput Component.
/// </summary>
    void OnAttack() 
    {
        Debug.Log("현재 비트 : " + currentMusicBeat);
        
        float accuracy = 1 - Mathf.Abs(musicManager.GetTimingOffset() / (musicManager.noteInterval));

        Debug.Log("정확도 : " + accuracy);

        if(accuracy > 0.7 && GetIsAttackBeatCount(currentMusicBeat - 4) && !currentBeatInputted) //Perfect Attack.
        {
            currentBeatInputted = true;
            Instantiate(perfectText, accuracyPos);
        } 
        else if(accuracy > 0.4 && GetIsAttackBeatCount(currentMusicBeat - 4) && !currentBeatInputted)
        {   
            currentBeatInputted = true;
            Instantiate(goodText, accuracyPos);
        }
        else if(!GetIsAttackBeatCount(currentMusicBeat - 4) && !currentBeatInputted)
        {
            currentBeatInputted = true;
            Instantiate(breakText, accuracyPos);
        }
        else if(currentBeatInputted)
        {
            //Do Nothing?
        }

        
    }
}
