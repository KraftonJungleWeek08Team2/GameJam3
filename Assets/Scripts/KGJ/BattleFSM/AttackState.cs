using System;
using UnityEngine;

public class AttackState : ITurnState 
{
    SlotInfo _slotInfo;
    public AttackState(SlotInfo slotInfo)
    {
        _slotInfo = slotInfo;
    }

    public void EnterState()
    {
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatInputChecker>().OnAttackEvent += Attack;
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatInputChecker>().OnEndRhythmEvent += ChangeSkillState;
        Managers.InputManager.RhythmAttackEnable(true); // InputManager의 액션 맵을 RhythmAttack으로 변경
        Managers.TurnManager.BeatBarSystem.ActivateBeatBar(_slotInfo); // BeatBar 동작 시작
    }

    public void UpdateState()
    {

    }

    public void FixedUpdateState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.BeatBarSystem.DisableBeatBar(); // BeatBar 끄기
        Managers.TurnManager.SlotMachine.HideResult(); // SlotMachine 결과 숨기기
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatInputChecker>().OnAttackEvent -= Attack; // BeatBar 공격 액션 해제
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatInputChecker>().OnEndRhythmEvent -= ChangeSkillState;
        Managers.InputManager.RhythmAttackEnable(false); // InputManager의 액션 맵 구독 끊기
    }
    
    void ChangeSkillState(bool isSuccess)
    {
        Managers.TurnManager.ChangeState(new SkillState(_slotInfo, isSuccess));
    }

    void Attack(AccuracyType accuracy)
    {
        switch (accuracy)
        {
            case AccuracyType.Perfect:
                Managers.TurnManager.CurrentEnemy.TakeDamage(2);
                Managers.CameraManager.ShakeCamera();
                SoundManager.Instance.PlayPerfectSound();
                Managers.TurnManager.Player.Attack();
                break;
            case AccuracyType.Good:
                Managers.TurnManager.CurrentEnemy.TakeDamage(1);
                Managers.CameraManager.ShakeCamera();
                SoundManager.Instance.PlayGoodSound();
                Managers.TurnManager.Player.Attack();
                break;
            case AccuracyType.Miss:
                break;
        }
    }
    
}
