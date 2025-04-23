using UnityEngine;

public class SlotState : ITurnState
{
    public void EnterState()
    {
        // SlotMachine 시작함
        if (Managers.TurnManager.CurrentEnemy != null)
        {
            Managers.CameraManager.AddMember(Managers.TurnManager.CurrentEnemy.transform, 0.5f, 1f);
        }
        
        Managers.CameraManager.ChangeBattleCamera(true);

        Managers.TurnManager.SlotMachine.ShowSlotUI();
        Debug.Log("ShowSlotUI");
    }

    public void ExecuteState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.SlotMachine.HideSlotUI();
    } 
}
