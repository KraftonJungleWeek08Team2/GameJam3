using UnityEngine;

public class Jackpot : ISkill
{
    string description = "JackPot";

    public void Execute()
    {
        Debug.Log("KGJ : Jackpot!!!!!!!");
        Managers.SkillManager.SevenSkill();
        Managers.TurnManager.CurrentEnemy.TakeDamage(777);
        Managers.TurnManager.Player.TakeHeal(777);
        Managers.CameraManager.ShakeCamera();
        Managers.TurnManager.Player.Attack();

        ShowSkillDescriptionUI();
    }

    
    void ShowSkillDescriptionUI()
    {
        Managers.TurnManager.BeatBarPanelBehaviour.ShowSkillDescriptionUI(description);
    }
}
