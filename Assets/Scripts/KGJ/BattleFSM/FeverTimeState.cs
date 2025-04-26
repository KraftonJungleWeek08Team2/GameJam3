public class FeverTimeState : ITurnState
{
    bool _isSuccess;
    bool _isFever;
    string description = "Fever Time.";

    public FeverTimeState(bool isSuccess, bool isFever)
    {
        _isSuccess = isSuccess;
        _isFever = isFever;
    }

    // FeverTime에서 할 일 : isFever라면 FeverTime update 실행, 아니라면 바로 CheckingState로 넘겨주기
    public void EnterState()
    {
        if (!_isFever)
        {
            Managers.TurnManager.ChangeState(new CheckingState(_isSuccess));
            return;
        }
        // UI 켜주기 
        Managers.TurnManager.BeatBarPanelBehaviour.ShowSkillDescriptionUI(description);
        // InputManager 액션 구독 (ATTACK)
        Managers.InputManager.RhythmAttackEnable(true);
        Managers.TurnManager.FeverTimeController.ShowFeverTime();
        Managers.TurnManager.FeverTimeController.OnFeverAttackEvent += FeverAttack;
        Managers.TurnManager.FeverTimeController.OnFeverEndEvent += CheckState;
    }

    public void ExitState()
    {
        // UI 끄기
        // InputManager 액션 구독 해제
        // 액션 구독 해제
        Managers.InputManager.RhythmAttackEnable(false);
        Managers.TurnManager.FeverTimeController.HideFeverTime();
        Managers.TurnManager.FeverTimeController.OnFeverAttackEvent -= FeverAttack;
        Managers.TurnManager.FeverTimeController.OnFeverEndEvent -= CheckState;
    }

    public void FixedUpdateState()
    {
        
    }

    public void UpdateState()
    {
        Managers.TurnManager.FeverTimeController.UpdateFeverTimer();
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
        Managers.TurnManager.ChangeState(new CheckingState(_isSuccess));
    }
}
