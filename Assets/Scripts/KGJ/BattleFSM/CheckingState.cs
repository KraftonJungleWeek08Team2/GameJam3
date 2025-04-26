using UnityEngine;

public class CheckingState : ITurnState
{
    public void EnterState()
    {
        throw new System.NotImplementedException();
    }

    public void ExitState()
    {
        throw new System.NotImplementedException();
    }

    public void FixedUpdateState()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    void CheckState(bool isSuccess)
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
            if (isSuccess)
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
