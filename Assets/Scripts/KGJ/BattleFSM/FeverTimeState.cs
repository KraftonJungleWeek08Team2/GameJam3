public class FeverTimeState : ITurnState
{
    bool _isSuccess;
    string description = "Fever Time.";

    public FeverTimeState(bool isSuccess)
    {
        _isSuccess = isSuccess;
    }

    public void EnterState()
    {
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
}
