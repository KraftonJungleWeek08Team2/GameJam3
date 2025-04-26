public class Heal : ISkill
{
    private int healValue;
    public Heal(int value)
    {
        healValue = value;
    }
    public void Execute()
    {
        //TODO : 힐 이펙트 
        Managers.SkillManager.HealSkill();
        //TODO : 힐 사운드
        Managers.TurnManager.Player.TakeHeal(healValue);
    }
}
