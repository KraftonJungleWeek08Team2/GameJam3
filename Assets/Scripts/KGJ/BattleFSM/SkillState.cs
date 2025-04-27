/// <summary>
/// SkillState에서 하는 일 : 스킬켜고 적에게 데미지 주고 바로 FeverTime으로 넘겨주기 
/// </summary>
public class SkillState : ITurnState
{
    SlotInfo _slotInfo;
    bool _isSuccess;

    public SkillState(SlotInfo slotInfo, bool isSuccess)
    {
        _slotInfo = slotInfo;
        _isSuccess = isSuccess;
    }

    public void EnterState()
    {
        if (_isSuccess)
        {
            HandleSuccessfulSkill();
        }
        else
        {
            Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess, null));
        }
    }

    void HandleSuccessfulSkill()
    {
        CombinationType? combi = CombinationChecker.Check(_slotInfo);

        if (combi != CombinationType.Sequential)
        {
            Managers.TurnManager.SkillBook.TryActivateSkill(combi);
        }
        Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess, combi));
    }

    public void ExitState()
    {
        
    }

    public void FixedUpdateState() { }
    public void UpdateState() { }
}
