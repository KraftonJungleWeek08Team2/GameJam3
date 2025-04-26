public class ThreeOddSkill : ISkill
{
    int _damage;

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
    }
}
