using UnityEngine;

public class BeatInputChecker : MonoBehaviour
{
    BeatBarSystem beatBarSystem;

    void Start()
    {
        beatBarSystem = GetComponent<BeatBarSystem>();

        beatBarSystem.OnEnableBeatBarAction += EnableInput;
        beatBarSystem.OnDisableBeatBarAction += DisableInput;
    
    }

    void EnableInput()
    {
        Managers.InputManager.OnRhythmAttackEvent += Attack;
    }

    void DisableInput()
    {
        Managers.InputManager.OnRhythmAttackEvent -= Act;
    }

    void OnDestroy()
    {
        UnsubscribeAction();      
    }

    void UnsubscribeAction()
    {
        //Action
        beatBarSystem.OnEnableBeatBarAction -= EnableInput;
        beatBarSystem.OnDisableBeatBarAction -= DisableInput;

        //Input
        Managers.InputManager.OnRhythmAttackEvent -= Attack;
    }

    private float CheckAccuracyWithCurrentBeat()
    {
        Debug.Log("현재 비트 : " + beatBarSystem.CurrentMusicBeat);
        
        float accuracy = 1 - Mathf.Abs(MusicManager.Instance.GetTimingOffset() / (MusicManager.Instance.beatInterval) / 2) + 0.05f;

        Debug.Log("정확도 : " + accuracy);

        return accuracy;

    }

    void Attack()
    {
        float accuracy = CheckAccuracyWithCurrentBeat();

        if(accuracy > 0.9 && GetIsAttackBeatCount(currentMusicBeat) && !currentBeatInputted) //Perfect Attack.
        {
            OnAttackEvent?.Invoke(AccuracyType.Perfect);

            currentBeatInputted = true;
            Instantiate(perfectText, accuracyPos);
            currentComboCount ++;
            
            if (currentMusicBeat == endBeat) //마지막 비트에 입력 성공.
            {
                Debug.Log("Log : 마지막 비트 입력 성공 퍼펙트");
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }

        } 
        else if(accuracy > 0.6 && GetIsAttackBeatCount(currentMusicBeat) && !currentBeatInputted)
        {
            OnAttackEvent?.Invoke(AccuracyType.Good);

            currentBeatInputted = true;
            
            Instantiate(goodText, accuracyPos);
            currentComboCount ++;
            if (currentMusicBeat == endBeat) //마지막 비트에 입력 성공.
            {
                Debug.Log("Log : 마지막 비트 입력 성공");
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }

        }
        else if(accuracy <= 0.6 && GetIsAttackBeatCount(currentMusicBeat) && !currentBeatInputted) //bad.
        {
            OnAttackEvent?.Invoke(AccuracyType.Miss);
            currentBeatInputted = true;
            isFullCombo = false;
            Instantiate(breakText, accuracyPos);
            currentComboCount = 0;
            if (currentMusicBeat == endBeat) //마지막 비트에 입력 : Bad.
            {
                Debug.Log("Log : 마지막 비트 입력 bad.");
                OnEndRhythmEvent?.Invoke(isFullCombo);
            }
        }
        else if(!GetIsAttackBeatCount(currentMusicBeat) && !currentBeatInputted) //아닌데 누름.
        {
            OnAttackEvent?.Invoke(AccuracyType.Miss);
            currentBeatInputted = true;
            isFullCombo = false;
            currentComboCount = 0;
            //Debug.Log("miss input.");
            Instantiate(breakText, accuracyPos);
        }
        else if(currentBeatInputted) //중복 입력
        {   

            Debug.Log("Duplicate input!!!!");
            isFullCombo = false;
            Instantiate(breakText, accuracyPos);
            currentComboCount = 0;
        }

        comboCountBehaviour.UpdateComboCount(currentComboCount);
    }
    }



}
