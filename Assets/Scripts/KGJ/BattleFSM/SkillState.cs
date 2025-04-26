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
        if (IsEnemyDead())
        {
            EnemyDieAndRespawn();
            Managers.TurnManager.ChangeState(new MoveState());
            return;
        }

        if (_isSuccess)
        {
            HandleSuccessfulSkill();
        }
        else
        {
            HandleFailedSkill();
        }
    }

    void HandleSuccessfulSkill()
    {
        CombinationType? combi = CombinationChecker.Check(_slotInfo);

        if (combi == null)
        {
            EvaluateAfterSkill();
        }
        else if (combi == CombinationType.Sequential)
        {
            Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess));
        }
        else
        {
            _skillBook.TryActivateSkill(combi);
            EvaluateAfterSkill();
        }
    }

    void HandleFailedSkill()
    {
        Managers.TurnManager.ChangeState(new KnockBackState());
    }

    void EvaluateAfterSkill()
    {
        // TODO : 적 데미지를 준후에 적 HP를 검사해야함
        if (IsEnemyDead())
        {
            EnemyDieAndRespawn();
            Managers.TurnManager.ChangeState(new MoveState());
        }
        else
        {
            if (_isSuccess)
            {
                Managers.TurnManager.BeatBarPanelBehaviour.OneMoreUIBehaviour.Show();
                Managers.TurnManager.ChangeState(new SlotState());
            }
            else
            {
                Managers.TurnManager.ChangeState(new KnockBackState());
            }
        }
    }

    bool IsEnemyDead()
    {
        return Managers.TurnManager.CurrentEnemy.hp <= 0;
    }

    void EnemyDieAndRespawn()
    {
        Managers.TurnManager.EnemyHpUI.HideEnemyUI();
        Managers.CameraManager.RemoveMember(Managers.TurnManager.CurrentEnemy.transform);
        Managers.TurnManager.CurrentEnemy.Die();

        Managers.TurnManager.CurrentEnemy = Managers.TurnManager.EnemySpawner.Spawn();
        Managers.TurnManager.CurrentEnemyIndex++;
        Managers.CameraManager.AddMember(Managers.TurnManager.CurrentEnemy.transform, 0.5f, 1f);
    }

    public void ExitState() { }
    public void FixedUpdateState() { }
    public void UpdateState() { }
}
