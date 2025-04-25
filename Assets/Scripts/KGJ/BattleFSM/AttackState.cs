public class AttackState : ITurnState
{
    SlotInfo _slotInfo;

    public AttackState(SlotInfo slotInfo)
    {
        _slotInfo = slotInfo;
    }

    public void EnterState()
    {
        Managers.InputManager.RhythmAttackEnable(true); // InputManager의 액션 맵을 RhythmAttack으로 변경
        // TODO : 생성자에서 받아온 slotInfo 값을 beatbarpanel에 넘겨주기
        Managers.TurnManager.BeatBarPanelBehaviour.ShowBeatBar(Managers.TurnManager.SlotMachine.SlotInfo); // BeatBar 동작 시작
    }

    public void UpdateState()
    {

    }

    public void FixedUpdateState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.BeatBarPanelBehaviour.HideBeatBar(); // BeatBar 끄기
        Managers.InputManager.RhythmAttackEnable(false); // InputManager의 액션 맵 구독 끊기
        Managers.TurnManager.SlotMachine.HideResult(); // SlotMachine 결과 숨기기
    }
}