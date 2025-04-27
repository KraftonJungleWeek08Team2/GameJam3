/// <summary>
/// FeverTimeState에서 하는 일 : isFever라면 FeverTime update 실행, 아니라면 바로 CheckingState로 넘겨주기
/// </summary>
public class FeverTimeState : ITurnState
{
    bool _isSuccess;
    CombinationType? _combi;
    string description = "Fever Time.";
    

    // TODO
    // OnEndRhythmEvent가 Invoke되었을 때, IntervalTimer를 시작하고, IntervalTimer가 끝났을때, CheckState 실행해서 다음으로 넘겨준다.
    public FeverTimeState(bool isSuccess, CombinationType? combi)
    {
        _isSuccess = isSuccess;
        _combi = combi;
    }

    public void EnterState()
    {
        if (_combi == CombinationType.Sequential)
        { 
            // UI 켜주기 
            Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().ShowSkillDescriptionUI(description);
            // InputManager 액션 구독 (ATTACK)
            Managers.InputManager.RhythmAttackEnable(true);
            Managers.TurnManager.FeverTimeController.ShowFeverTime();
            Managers.TurnManager.FeverTimeController.OnFeverAttackEvent += FeverAttack;
            Managers.TurnManager.FeverTimeController.OnFeverEndEvent += CheckState;
        }
        else
        {
            CheckState();
        }
    }

    public void ExitState()
    {
        // UI 끄기
        // InputManager 액션 구독 해제
        // 액션 구독 해제
        if (_combi == CombinationType.Sequential)
        {
            Managers.InputManager.RhythmAttackEnable(false);
            Managers.TurnManager.FeverTimeController.HideFeverTime();
            Managers.TurnManager.FeverTimeController.OnFeverAttackEvent -= FeverAttack;
            Managers.TurnManager.FeverTimeController.OnFeverEndEvent -= CheckState;
        }
    }

    public void FixedUpdateState()
    {
        
    }

    public void UpdateState()
    {
        if (_combi == CombinationType.Sequential)
        {
            Managers.TurnManager.FeverTimeController.UpdateFeverTimer();
        }
    }

    void FeverAttack()
    {
        Managers.TurnManager.Player.Attack();
        Managers.TurnManager.CurrentEnemy.TakeDamage(2);
        Managers.CameraManager.ShakeCamera();
        SoundManager.Instance.PlayPerfectSound();
    }

    void CheckState()
    {
        Managers.TurnManager.ChangeState(new CheckingState(_isSuccess, _combi));
    }
}
