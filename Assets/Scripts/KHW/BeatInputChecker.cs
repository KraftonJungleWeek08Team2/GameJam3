using System;
using System.Collections.Generic;
using UnityEngine;

public class BeatInputChecker : MonoBehaviour
{
    BeatBarSystem beatBarSystem; //비트바 시스템
    BeatBarUISystem beatBarUISystem;


    private bool isWorking;
    private int currentCombo;
    private bool isFullCombo;
    public Action<bool> OnEndRhythmEvent;       // 리듬 공격 전체가 끝날 때의 액션, 풀콤 여부를 변수로 넘겨줌
    public Action<AccuracyType> OnAttackEvent;  // 각 리듬 공격의 판정을 enmu으로 넘겨줌

    void Start()
    {
        beatBarSystem = GetComponent<BeatBarSystem>(); //비트바 시스템.
        beatBarUISystem = GetComponent<BeatBarUISystem>(); //UI 점근

        beatBarSystem.OnEnableBeatBarAction += EnableInputChecker; //비트바 시스템 활성화시 클래스 활성화
        beatBarSystem.OnDisableBeatBarAction += DisableInputChecker; //비트바 시스템 비활성화시 클래스 비활성화

        isWorking = false;
        currentCombo = 0;
    }

    void EnableInputChecker()
    {
        EnableInputSystem();
        SubscribeAction();
        InitializeProperties();
    }

    void InitializeProperties()
    {
        isFullCombo = true;
        isWorking = true;
    }

    void DisableInputChecker()
    {
        DisableInputSystem();
        UnsubscribeAction();

        isWorking = false;
    }

    void EnableInputSystem()
    {
        Managers.InputManager.OnRhythmAttackEvent += Attack;
    }

    void DisableInputSystem()
    {
        Managers.InputManager.OnRhythmAttackEvent -= Attack;
    }



    void SubscribeAction()
    {
        beatBarSystem.OnEnableBeatBarAction += EnableInputChecker;
        beatBarSystem.OnDisableBeatBarAction += DisableInputChecker;
    }
    void UnsubscribeAction()
    {
        //Action
        beatBarSystem.OnEnableBeatBarAction -= EnableInputSystem;
        beatBarSystem.OnDisableBeatBarAction -= DisableInputSystem;
    }

    private float CheckAccuracyWithCurrentBeat()
    {
        Debug.Log("현재 비트 : " + beatBarSystem.CurrentMusicBeat);
        
        float accuracy = 1 - Mathf.Abs(MusicManager.Instance.GetTimingOffset(beatBarSystem.currentNote.OffsetBeat) / 0.4f) + 0.05f;

        Debug.Log("정확도 : " + accuracy);

        return accuracy;

    }

    void Attack()
    {
        float accuracy = CheckAccuracyWithCurrentBeat();

        if(accuracy > 0.9) //Perfect Attack.
        {
            OnAttackEvent?.Invoke(AccuracyType.Perfect);
            beatBarUISystem.ShowPerfectText();
            currentCombo++;
            
            if(beatBarSystem.currentNote.IsLast)
            {
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }

        } 
        else if(accuracy > 0.6)
        {
            OnAttackEvent?.Invoke(AccuracyType.Good); 
            beatBarUISystem.ShowGoodText();
            currentCombo++;

            if(beatBarSystem.currentNote.IsLast)
            {
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }
        }
        else if(accuracy <= 0.6 && accuracy > 0.05) //bad.
        {
            OnAttackEvent?.Invoke(AccuracyType.Miss);
            isFullCombo = false;
            beatBarUISystem.ShowBreakText();
            currentCombo = 0;
            if(beatBarSystem.currentNote.IsLast)
            {
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }


        }
        else //상관없는 입력
        {
            //Do Nothing.
        }
        
        beatBarUISystem.ShowComboCount(currentCombo);
    }

    /// <summary> 노트의 소유권을 넘깁니다. </summary>
    void ChangeCurrentNote()
    {
        beatBarSystem.ChangeCurrentNote();
    }

    void Update()
    {
        if(isWorking)
        {
            CheckPassedCurrentBeat();
        }

    }

    private void CheckPassedCurrentBeat()
    {
        float currentOffsetFromCurrentBeat = MusicManager.Instance.GetTimingOffset(beatBarSystem.currentNote.OffsetBeat);

        if(currentOffsetFromCurrentBeat > 0.4) //지나침!
        {
            ChangeCurrentNote();
            beatBarUISystem.ShowBreakText();
            currentCombo = 0;
            beatBarUISystem.ShowComboCount(currentCombo);
        }
    }

    void OnDestroy()
    {
        UnsubscribeAction();  //파괴시 구독 해제.
    }



}
