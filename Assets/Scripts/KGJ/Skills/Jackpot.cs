public class Jackpot : ISkill
{
    public void Execute()
    {
        Managers.SkillManager.SevenSkill();
        Managers.TurnManager.CurrentEnemy.TakeDamage(777);
        Managers.TurnManager.Player.TakeHeal(777);
        Managers.CameraManager.ShakeCamera();
        Managers.TurnManager.Player.Attack();
    }
}
