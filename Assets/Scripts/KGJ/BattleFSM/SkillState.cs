using UnityEngine;

public class SkillState : ITurnState
{
    SlotInfo _slotInfo;
    bool _isSuccess;
    SkillBook _skillBook;

    public SkillState(SlotInfo slotInfo, bool isSuccess)
    {
        _slotInfo = slotInfo;
        _isSuccess = isSuccess;
        _skillBook = new SkillBook();
    }

    public void EnterState()
    {
        if (_isSuccess)
        {
            switch (CombinationChecker.Check(_slotInfo))
            {
                case CombinationType.Jackpot:
                    Managers.TurnManager.SkillManager.SevenSkill();
                    break;
                case CombinationType.ThreeOfAKindOdd:
                    //Managers.TurnManager.SkillManager.
                    break;
                case CombinationType.ThreeOfAKindEven:
                    //Managers.TurnManager.SkillManager.
                    break;
                case CombinationType.Sequential:
                    Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess));
                    break;
                case CombinationType.AllOdd:

                    break;
                case CombinationType.AllEven:

                    break;
                case null:
                    CheckState();
                    break;
            }
        }
        else
        {
            Managers.TurnManager.ChangeState(new KnockBackState());
        }
    }

    public void ExitState()
    {

    }

    public void FixedUpdateState()
    {

    }

    public void UpdateState()
    {

    }

    void CheckState()
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
            if (_isSuccess)
            {
                // 풀 콤보라면, 다시 슬롯머신 상태로
                // 원 모어 UI를 띄우고, 슬롯머신 시간도 1초 줄임
                Managers.TurnManager.BeatBarPanelBehaviour.OneMoreUIBehaviour.Show();
                Managers.TurnManager.ChangeState(new SlotState());
            }
            else
            {
                // 풀 콤보가 아니라면, 플레이어가 데미지를 입고 넉백되며 다시 MoveState로 전환
                Managers.TurnManager.ChangeState(new KnockBackState());
            }
        }
    }
}
