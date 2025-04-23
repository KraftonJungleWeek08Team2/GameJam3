public class AttackState : ITurnState
{
    public void EnterState()
    {
        Managers.TurnManager.BeatBarPanelBehaviour.ShowBeatBar(Managers.TurnManager.SlotMachine.SlotInfo); // BeatBar 동작 시작
        Managers.InputManager.RhythmAttackEnable(true); // InputManager의 액션 맵을 RhythmAttack으로 변경
    }

    public void ExecuteState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.BeatBarPanelBehaviour.HideBeatBar(); // BeatBar 끄기
        Managers.InputManager.RhythmAttackEnable(false); // InputManager의 액션 맵 구독 끊기
    }
}