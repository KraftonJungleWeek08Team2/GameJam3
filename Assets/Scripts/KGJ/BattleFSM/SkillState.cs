/// <summary>
/// SkillState에서 하는 일 : 스킬켜고 적에게 데미지 주고 바로 FeverTime으로 넘겨주기 
/// </summary>
public class SkillState : ITurnState
{
    bool _isSuccess;
    CombinationType? _combi;

    public SkillState(CombinationType? combi, bool isSuccess)
    {
        _combi = combi;
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
        if (_combi != CombinationType.Sequential)
        {
            Managers.TurnManager.SkillBook.TryActivateSkill(_combi);
        }
        Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess, _combi));
    }

    public void ExitState()
    {
        
    }

    public void FixedUpdateState() { }
    public void UpdateState() { }
}
