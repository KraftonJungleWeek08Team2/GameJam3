using UnityEngine;

public class AttackState : ITurnState 
{
    SlotInfo _slotInfo;
    CombinationType? _combi;
    bool _isUIActive;

    public AttackState(SlotInfo slotInfo)
    {
        _slotInfo = slotInfo;
        _combi = CombinationChecker.Check(slotInfo);
    }

    public void EnterState()
    {
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatInputChecker>().OnAttackEvent += Attack;
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatInputChecker>().OnEndRhythmEvent += ChangeSkillState;
        Managers.InputManager.RhythmAttackEnable(true); // InputManager의 액션 맵을 RhythmAttack으로 변경
        Managers.TurnManager.BeatBarSystem.ActivateBeatBar(_slotInfo); // BeatBar 동작 시작
        // _combi 스킬에 맞는 ui 띄워주기, ui없으면 안띄워주기
        Managers.TurnManager.SkillBook.TryShowSkillDescriptionUI(_combi);
        _isUIActive = true;
    }

    public void UpdateState()
    {

    }

    public void FixedUpdateState()
    {

    }

    public void ExitState()
    {
        Managers.TurnManager.BeatBarSystem.DisableBeatBar(); // BeatBar 끄기
        Managers.TurnManager.SlotMachine.HideResult(); // SlotMachine 결과 숨기기
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatInputChecker>().OnAttackEvent -= Attack; // BeatBar 공격 액션 해제
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatInputChecker>().OnEndRhythmEvent -= ChangeSkillState;
        Managers.InputManager.RhythmAttackEnable(false); // InputManager의 액션 맵 구독 끊기
    }
    
    void ChangeSkillState(bool isSuccess)
    {
        Managers.TurnManager.ChangeState(new SkillState(_combi, isSuccess));
    }

    void Attack(AccuracyType accuracy)
    {
        switch (accuracy)
        {
            case AccuracyType.Perfect:
                Managers.TurnManager.CurrentEnemy.TakeDamage(2);
                Managers.CameraManager.ShakeCamera();
                SoundManager.Instance.PlayPerfectSound();
                Managers.TurnManager.Player.Attack();
                break;
            case AccuracyType.Good:
                Managers.TurnManager.CurrentEnemy.TakeDamage(1);
                Managers.CameraManager.ShakeCamera();
                SoundManager.Instance.PlayGoodSound();
                Managers.TurnManager.Player.Attack();
                break;
            case AccuracyType.Miss:
                if (_isUIActive)
                {
                    Debug.Log("[KGJ] TryHideSkillDescriptionUI");
                    Managers.TurnManager.SkillBook.TryHideSkillDescriptionUI(_combi);
                    _isUIActive = false;
                }
                break;
        }
    }
    
}
