using UnityEngine;

public class Heal : ISkill
{
    private int healValue;
    public Heal(int value)
    {
        healValue = value;
    }
    public void Execute()
    {
        Managers.TurnManager.Player.TakeHeal(healValue);
    }
}
