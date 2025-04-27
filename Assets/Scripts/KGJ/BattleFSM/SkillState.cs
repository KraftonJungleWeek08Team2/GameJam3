/// <summary>
/// SkillState에서 하는 일 : 스킬켜고 적에게 데미지 주고 바로 FeverTime으로 넘겨주기 
/// </summary>
public class SkillState : ITurnState
{
    SlotInfo _slotInfo;
    bool _isSuccess;
    bool _isFever;

    public SkillState(SlotInfo slotInfo, bool isSuccess)
    {
        _slotInfo = slotInfo;
        _isSuccess = isSuccess;
        _isFever = false;
    }

    public void EnterState()
    {
        if (_isSuccess)
        {
            HandleSuccessfulSkill();
        }
        else
        {
            Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess, _isFever));
        }
    }

    void HandleSuccessfulSkill()
    {
        CombinationType? combi = CombinationChecker.Check(_slotInfo);

        if (combi == CombinationType.Sequential)
        {
            _isFever = true;
        }
        else
        {
            Managers.TurnManager.SkillBook.TryActivateSkill(combi);
        }
        Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess, _isFever));
    }

    public void ExitState()
    {
        
    }

    public void FixedUpdateState() { }
    public void UpdateState() { }
}
