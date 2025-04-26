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

    public void EnterState()
    {
        Managers.TurnManager.SkillBook.OnCompleteEvent += EvaluateAfterSkill;

        // TODO : Enemy가 죽었어도 스킬은 발동, 하지만 OneMoreUI는 발동하지 않음
        if (_isSuccess)
        {
            HandleSuccessfulSkill();
        }
        else
        {
            if (IsEnemyDead())
            {
                HandleSuccessfulSkill();
            }
            else
            {
                Managers.TurnManager.ChangeState(new KnockBackState());
            }
        }
    }

    void HandleSuccessfulSkill()
    {
        CombinationType? combi = CombinationChecker.Check(_slotInfo);

        if (combi == null)
        {
            Debug.Log("[KGJ] Skill is null");
            EvaluateAfterSkill();
        }
        else if (combi == CombinationType.Sequential)
        {
            Debug.Log("[KGJ] Skill is sequential");
            Managers.TurnManager.ChangeState(new FeverTimeState(_isSuccess));
        }
        else
        {
            Debug.Log($"[KGJ] SkillState : {combi.Value.ToString()}");
            Managers.TurnManager.SkillBook.TryActivateSkill(combi);
        }
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
                Debug.Log($"[KGJ] SkillState : CurrentEnemy HP {Managers.TurnManager.CurrentEnemy.hp}");
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

    public void ExitState()
    {
        Managers.TurnManager.SkillBook.OnCompleteEvent -= EvaluateAfterSkill;
    }

    public void FixedUpdateState() { }
    public void UpdateState() { }

    void OnDestory()
    {
        Managers.TurnManager.SkillBook.OnCompleteEvent -= EvaluateAfterSkill;
    }
}
