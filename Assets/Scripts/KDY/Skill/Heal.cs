public class Heal : ISkill
{
    private int healValue;
    private string description;
    public Heal(int value)
    {
        healValue = value;
        description = "체력을 약간 회복합니다.";
    }
    public void Execute()
    {
        //TODO : 힐 이펙트 
        Managers.SkillManager.HealSkill();
        //TODO : 힐 사운드
        Managers.TurnManager.Player.TakeHeal(healValue);
    }

    public void ShowSkillDescriptionUI()
    {
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().ShowSkillDescriptionUI(description);
    }
}
