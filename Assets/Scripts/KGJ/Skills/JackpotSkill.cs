public class JackpotSkill : ISkill
{
    string description = "JackPot";

    public void Execute()
    {
        Managers.SkillManager.SevenSkill();
        Managers.TurnManager.CurrentEnemy.TakeDamage(77);
        Managers.TurnManager.Player.TakeHeal(7);
        Managers.CameraManager.ShakeCamera();
        Managers.TurnManager.Player.Attack();
    }


    public void ShowSkillDescriptionUI()
    {
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().ShowSkillDescriptionUI(description);
    }
}