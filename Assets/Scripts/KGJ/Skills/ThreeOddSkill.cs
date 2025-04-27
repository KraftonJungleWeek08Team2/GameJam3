public class ThreeOddSkill : ISkill
{
    int _damage;
    string description = "큰 추가 대미지를 줍니다.";
    public ThreeOddSkill(int value)
    {
        _damage = value;
    }

    public void Execute()
    {
        Managers.SkillManager.AttackSkill();
        Managers.TurnManager.CurrentEnemy.TakeDamage(_damage);
        Managers.CameraManager.ShakeCamera();
        Managers.TurnManager.Player.Attack();

        //ShowSkillDescriptionUI();
    }

    public void ShowSkillDescriptionUI()
    {
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().ShowSkillDescriptionUI(description);
    }

    public void HideSkillDescriptionUI()
    {
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().HideSkillDescriptionUI();
    }
}
