using System;
using System.Collections.Generic;
using UnityEngine;


public enum AccuracyType
{
    Perfect,
    Good,
    Miss
}


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

        SubscribeAction();

        isWorking = false;
        currentCombo = 0;
    }

    void EnableInputChecker()
    {
        EnableInputSystem();
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
        //UnsubscribeAction();

        isWorking = false;
    }

    void EnableInputSystem()
    {
        // 기존 구독 해제 (중복 방지)
        Managers.InputManager.OnRhythmAttackEvent -= Attack;
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
        beatBarSystem.OnEnableBeatBarAction -= EnableInputChecker;
        beatBarSystem.OnDisableBeatBarAction -= DisableInputChecker;
    }

    private float CheckAccuracyWithCurrentBeat()
    {
        //Debug.Log("현재 비트 : " + beatBarSystem.CurrentMusicBeat + " " + beatBarSystem.currentNote.OffsetBeat);
        
        float accuracy = 1 - Mathf.Abs(MusicManager.Instance.GetTimingOffset(beatBarSystem.currentNote.OffsetBeat + beatBarSystem.currentNote.Beat)) / 0.2f;

        //Debug.Log("정확도 : " + accuracy);

        return accuracy;

    }

    void Attack()
    {
        float accuracy = CheckAccuracyWithCurrentBeat();
        Debug.Log($"KHW : 정확도 : {accuracy}");
        string currentNoteIndex = (beatBarSystem.currentNote.Beat + beatBarSystem.currentNote.OffsetBeat).ToString();
        //Debug.Log($"KHW : 현재 노트의 비트 : {beatBarSystem.currentNote.Beat + beatBarSystem.currentNote.OffsetBeat}");
        //Debug.Log($"KHW : 현재 노래의 비트 : {MusicManager.Instance.currentBeat}");

        if(accuracy > 0.8) //Perfect Attack.
        {
            OnAttackEvent?.Invoke(AccuracyType.Perfect);
            beatBarUISystem.ShowPerfectText();
            currentCombo++;
            beatBarUISystem.ShowComboCount(currentCombo);
            
            if(beatBarSystem.currentNote.IsLast)
            {
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }
            DisableCurrentNote(currentNoteIndex, AccuracyType.Perfect);
            ChangeCurrentNote();
        } 
        else if(accuracy > 0.5)
        {
            OnAttackEvent?.Invoke(AccuracyType.Good); 
            beatBarUISystem.ShowGoodText();
            currentCombo++;

            beatBarUISystem.ShowComboCount(currentCombo);
            if(beatBarSystem.currentNote.IsLast)
            {
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }

            DisableCurrentNote(currentNoteIndex, AccuracyType.Good);
            ChangeCurrentNote();
        }
        else if(accuracy <= 0.5 && accuracy > 0.05) //bad.
        {
            OnAttackEvent?.Invoke(AccuracyType.Miss);
            isFullCombo = false;
            beatBarUISystem.ShowBreakText();
            currentCombo = 0;

            beatBarUISystem.ShowComboCount(currentCombo);
            if(beatBarSystem.currentNote.IsLast)
            {
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }

            DisableCurrentNote(currentNoteIndex, AccuracyType.Miss);
            ChangeCurrentNote();
        }
        else //상관없는 입력
        {
            //Do Nothing.
        }
        

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
        float currentOffsetFromCurrentBeat = MusicManager.Instance.GetTimingOffset(beatBarSystem.currentNote.OffsetBeat + beatBarSystem.currentNote.Beat);
        string currentNoteIndex = (beatBarSystem.currentNote.Beat + beatBarSystem.currentNote.OffsetBeat).ToString();
        //Debug.Log($"KHW : 현재 노트의 비트 : {beatBarSystem.currentNote.Beat + beatBarSystem.currentNote.OffsetBeat}");

        if(currentOffsetFromCurrentBeat > 0.2) //지나침!
        {
            OnAttackEvent?.Invoke(AccuracyType.Miss);
            DisableCurrentNote(currentNoteIndex, AccuracyType.Miss);
            ChangeCurrentNote();
            beatBarUISystem.ShowBreakText();
            currentCombo = 0;
            beatBarUISystem.ShowComboCount(currentCombo);
            isFullCombo = false;
            if(beatBarSystem.currentNote.IsLast)
            {
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }
        }
    }

    void DisableCurrentNote(string index, AccuracyType accuracyType)
    {
        beatBarUISystem.NoteInputted(index, accuracyType);
    }

    void OnDestroy()
    {
        UnsubscribeAction();
        DisableInputSystem(); // 입력 이벤트 구독 해제
    }



}
