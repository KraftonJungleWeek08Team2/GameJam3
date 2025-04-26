using UnityEngine;

public class CheckingState : ITurnState
{
    // CheckingState에서 할 일 : 풀콤, 적 체력에 따라 knockback, move, slot으로 넘겨주기
    bool _isSuccess;

    public CheckingState(bool isSuccess)
    {
        _isSuccess = isSuccess;
    }

    public void EnterState()
    {
        CheckState();
    }

    public void ExitState()
    {

    }

    public void FixedUpdateState() { }
    public void UpdateState() { }

    void CheckState()
    {
        // 적 hp와 풀콤 여부와 슬롯 스킬 체크해서 상태 변경, onemore ui도 여기서 제어
        if (Managers.TurnManager.CurrentEnemy.hp <= 0)
        {
            Debug.Log("[KGJ] HP <= 0");
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
                // 원 모어 UI를 띄움
                Debug.Log("[KGJ] HP > 0 && FullCombo");
                Managers.TurnManager.BeatBarPanelBehaviour.OneMoreUIBehaviour.Show();
                Managers.TurnManager.ChangeState(new SlotState());
            }
            else
            {
                Debug.Log("[KGJ] HP > 0 && not FullCombo");
                // 풀 콤보가 아니라면, 플레이어가 데미지를 입고 넉백되며 다시 MoveState로 전환
                Managers.TurnManager.ChangeState(new KnockBackState());
            }
        }
    }
}
