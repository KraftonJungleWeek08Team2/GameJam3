public class AttackState : ITurnState
{
    public void EnterState()
    {

        // RhythmAttack에 slotInfo을 넘겨서 시작하기
        // Debug.Log($"{_slotInfo.GetValue(0)}, {_slotInfo.GetValue(1)}, {_slotInfo.GetValue(2)}");
        Managers.TurnManager.BeatBarPanelBehaviour.ShowBeatBar(Managers.TurnManager.SlotMachine.SlotInfo);
        Managers.InputManager.RhythmAttackEnable(true);
    }

    public void ExecuteState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.BeatBarPanelBehaviour.HideBeatBar();
        Managers.InputManager.RhythmAttackEnable(false);

    }
}