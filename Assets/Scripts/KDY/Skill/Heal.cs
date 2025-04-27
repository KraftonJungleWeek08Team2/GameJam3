using System;

public class Heal : ISkill
{
    private int healValue;
    private string description = "Restore HP.";
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
