using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// CheckingState에서 할 일 : 풀콤, 적 체력에 따라 knockback, move, slot으로 넘겨주기, 스킬 후 딜레이
/// </summary>
public class CheckingState : ITurnState
{
    bool _isSuccess;
    CombinationType? _combi;

    // 대기 시간 관련 변수
    float _intervalTimer = 0;
    bool _isTimerRunning = false;
    const float INTERVAL = 1.25f;

    ITurnState _nextState;

    public CheckingState(bool isSuccess, CombinationType? combi)
    {
        _isSuccess = isSuccess;
        _combi = combi;
    }

    public void EnterState()
    {
        if (_combi == null)
        {
            // 스킬 조합이 없다면 딜레이 업승ㅁ
            CheckState();
            ChangeNextState();
        }
        else
        {
            // 스킬 조합이 존재하거나 풀콤보 실패 시 딜레이 존재
            InitTimer();
            CheckState();
            _isTimerRunning = true;
        }
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().HideSkillDescriptionUI();
    }

    public void ExitState()
    {

    }

    public void FixedUpdateState() { }
    public void UpdateState() 
    {
        if (_isTimerRunning)
        {
            UpdateTimer();
        }
    }

    void InitTimer()
    {
        _intervalTimer = 0;
        _isTimerRunning = false;
    }

    void UpdateTimer()
    {
        _intervalTimer += Time.deltaTime;
        if (_intervalTimer >= INTERVAL)
        {
            ChangeNextState();
        }
    }

    void ChangeNextState()
    {
        if (_nextState is MoveState)
        {
            Managers.TurnManager.CurrentEnemy.Die();
            // EnemySpawner에서 적을 생성하고 CurrentEnemy에 넣어줌
            Managers.TurnManager.CurrentEnemy = Managers.TurnManager.EnemySpawner.Spawn();
            Managers.TurnManager.CurrentEnemyIndex++;
            Managers.CameraManager.AddMember(Managers.TurnManager.CurrentEnemy.transform, 0.5f, 1f);
        }
        else if (_nextState is SlotState)
        {
            Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().ShowOneMoreUI();
        }
        
        Managers.TurnManager.ChangeState(_nextState);
    }

    void CheckState()
    {
        if (Managers.TurnManager.CurrentEnemy.hp <= 0)
        {
            Managers.TurnManager.EnemyHpUI.HideEnemyUI(); // 적 체력 UI 숨기기
            Managers.CameraManager.RemoveMember(Managers.TurnManager.CurrentEnemy.transform);
            Managers.TurnManager.CurrentEnemy.PlayDeath();

            
            _nextState = new MoveState();
        }
        else
        {
            if (_isSuccess)
            {
                _nextState = new SlotState();
            }
            else
            {
                _nextState = new KnockBackState();
            }
        }
    }
}
