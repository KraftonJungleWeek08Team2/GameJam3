using UnityEngine;

public class SkillState : ITurnState
{
    SlotInfo _slotInfo;
    bool _isSuccess;

    public SkillState(SlotInfo slotInfo, bool isSuccess)
    {
        _slotInfo = slotInfo;
        _isSuccess = isSuccess;
    }

    // TODO
    // SkillState에서 할 일 : 스킬켜고 적에게 데미지 주고 바로 FeverTime으로 넘겨주기 


    public void EnterState()
    {
        if (_isSuccess)
        {
            HandleSuccessfulSkill();
        }
        else
        {
            Managers.TurnManager.ChangeState(new FeverTimeState(false, false));
        }
    }

    void HandleSuccessfulSkill()
    {
        CombinationType? combi = CombinationChecker.Check(_slotInfo);

        if (combi == null)
        {
            Debug.Log("[KGJ] Skill is null");
            Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess, false));
        }
        else if (combi == CombinationType.Sequential)
        {
            Debug.Log("[KGJ] Skill is sequential");
            Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess, true));
        }
        else
        {
            Debug.Log($"[KGJ] SkillState : {combi.Value.ToString()}");
            Managers.TurnManager.SkillBook.TryActivateSkill(combi);
            Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess, false));
        }
    }

    public void ExitState()
    {
        
    }

    public void FixedUpdateState() { }
    public void UpdateState() { }
}
