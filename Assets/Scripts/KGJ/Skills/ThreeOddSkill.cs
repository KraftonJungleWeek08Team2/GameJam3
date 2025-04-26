
using System;

public class ThreeOddSkill : ISkill
{
    int _damage;
    string description = "Deal Huge Amount Of Additional Damage";
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

        ShowSkillDescriptionUI();
    }

    void ShowSkillDescriptionUI()
    {
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().ShowSkillDescriptionUI(description);
    }
}
