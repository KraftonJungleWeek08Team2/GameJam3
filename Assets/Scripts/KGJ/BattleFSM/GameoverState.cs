public class GameoverState : ITurnState
{
    public void EnterState()
    {
        Managers.CameraManager.ChangeBattleCamera(false);
        if (Managers.TurnManager.CurrentEnemy != null)
        {
            Managers.TurnManager.CurrentEnemy.isMoving = false;
        }
        Managers.TurnManager.SlotMachine.HideSlotUI();
        Managers.TurnManager.SlotMachine.HideResult();
        Managers.TurnManager.BeatBarPanelBehaviour.HideBeatBar();
        Managers.TurnManager.ParallaxBackground.IsScrolling = false;
        Managers.InputManager.Clear();
    }

    public void ExitState()
    {
        
    }

    public void FixedUpdateState()
    {

    }

    public void UpdateState()
    {

    }
}
