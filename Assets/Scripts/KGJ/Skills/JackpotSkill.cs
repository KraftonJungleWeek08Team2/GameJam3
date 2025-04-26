using System;
using UnityEngine;

public class JackpotSkill : ISkill
{
    string description = "JackPot";

    public void Execute()
    {
        Managers.SkillManager.SevenSkill();
        Managers.TurnManager.CurrentEnemy.TakeDamage(777);
        Managers.TurnManager.Player.TakeHeal(777);
        Managers.CameraManager.ShakeCamera();
        Managers.TurnManager.Player.Attack();

        ShowSkillDescriptionUI();
    }


    void ShowSkillDescriptionUI()
    {
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().ShowSkillDescriptionUI(description);
    }
}