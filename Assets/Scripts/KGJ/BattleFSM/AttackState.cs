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
        Managers.TurnManager.BeatBarPanelBehaviour.GetComponent<BeatInputChecker>().OnEndRhythmEvent += ChangeNextState;
        Managers.TurnManager.BeatBarPanelBehaviour.GetComponent<BeatInputChecker>().OnAttackEvent += Attack;
        Managers.TurnManager.BeatBarPanelBehaviour.GetComponent<BeatInputChecker>().OnEndRhythmEvent += ChangeSkillState;
        //Managers.TurnManager.BeatBarPanelBehaviour.GetComponent<BeatInputChecker>().OnEndRhythmEvent += Attack;
        //Managers.InputManager.RhythmAttackEnable(true); // InputManager의 액션 맵을 RhythmAttack으로 변경
        Managers.TurnManager.BeatBarPanelBehaviour.ActivateBeatBar(_slotInfo); // BeatBar 동작 시작
    }

    public void UpdateState()
    {

    }

    public void FixedUpdateState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.BeatBarPanelBehaviour.DisableBeatBar(); // BeatBar 끄기
        Managers.InputManager.RhythmAttackEnable(false); // InputManager의 액션 맵 구독 끊기
        Managers.TurnManager.SlotMachine.HideResult(); // SlotMachine 결과 숨기기
        Managers.TurnManager.BeatBarPanelBehaviour.GetComponent<BeatInputChecker>().OnEndRhythmEvent -= ChangeNextState; // BeatBar 종료 액션 해제
        Managers.TurnManager.BeatBarPanelBehaviour.GetComponent<BeatInputChecker>().OnAttackEvent -= Attack; // BeatBar 공격 액션 해제
        //Managers.TurnManager.BeatBarPanelBehaviour.GetComponent<BeatInputChecker>().OnEndRhythmEventOnEndRhythmEvent -= ChangeSkillState;
        //Managers.TurnManager.BeatBarPanelBehaviour.GetComponent<BeatInputChecker>().OnEndRhythmEventOnAttackEvent -= Attack; // BeatBar 공격 액션 해제
    }

    void ChangeNextState(bool isSuccess)
    {
        Debug.Log($"{_slotInfo.GetValue(0)}, {_slotInfo.GetValue(1)}, {_slotInfo.GetValue(2)}");
        if (isSuccess)
        {
            switch (CombinationChecker.Check(_slotInfo))
            {
                case CombinationType.Jackpot:

                    break;
                case CombinationType.ThreeOfAKindOdd:

                    break;
                case CombinationType.Sequential:
                    Managers.TurnManager.ChangeState(new FeverTimeState(isSuccess));
                    break;
                case CombinationType.AllOdd:

                    break;
                case CombinationType.AllEven:

                    break;
                case null:
                    CheckState(isSuccess);
                    break;
            }
        }
        else
        {
            Managers.TurnManager.ChangeState(new KnockBackState());
        }
    }

    void CheckState(bool isSuccess)
    {
        // 적 hp와 풀콤 여부와 슬롯 스킬 체크해서 상태 변경, onemore ui도 여기서 제어
        if (Managers.TurnManager.CurrentEnemy.hp <= 0)
        {
            Managers.TurnManager.EnemyHpUI.HideEnemyUI(); // 적 체력 UI 숨기기
            Managers.CameraManager.RemoveMember(Managers.TurnManager.CurrentEnemy.transform);
            Managers.TurnManager.CurrentEnemy.Die();

            // EnemySpawner에서 적을 생성하고 CurrentEnemy에 넣어줌
            Managers.TurnManager.CurrentEnemy = Managers.TurnManager.EnemySpawner.Spawn();
            Managers.TurnManager.CurrentEnemyIndex++;
            Managers.CameraManager.AddMember(Managers.TurnManager.CurrentEnemy.transform, 0.5f, 1f);
            Managers.TurnManager.ChangeState(new MoveState());

        }
        else
        {
            if (isSuccess)
            {
                // 풀 콤보라면, 다시 슬롯머신 상태로
                // 원 모어 UI를 띄우고, 슬롯머신 시간도 1초 줄임
                Managers.TurnManager.BeatBarPanelBehaviour.GetComponent<BeatBarUISystem>().ShowOneMoreUI();
                Managers.TurnManager.ChangeState(new SlotState());
            }
            else
            {
                // 풀 콤보가 아니라면, 플레이어가 데미지를 입고 넉백되며 다시 MoveState로 전환
                Managers.TurnManager.ChangeState(new KnockBackState());
            }
        }
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
