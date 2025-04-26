using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class OddSkill : ISkill
{
    private int damageValue;
    //스킬 이펙트 파티클
    private GameObject skillEffect;
    private string description = "Deal Additional Damage.";
    public OddSkill(int value)
    {
        damageValue = value;
        skillEffect = Resources.Load<GameObject>("KMS/SkillEffects/hit Odd");
    }
    public void Execute(Action onComplete)
    {
        // 적 위치에 스킬 이펙트 생성
        GameObject go = Object.Instantiate(skillEffect, Managers.TurnManager.CurrentEnemy.transform.position, Quaternion.identity);
        Object.Destroy(go, 5f);
        
        //TODO : 홀수 스킬 사운드
        Managers.TurnManager.CurrentEnemy.TakeDamage(damageValue);
        Managers.CameraManager.ShakeCamera();
        Managers.TurnManager.Player.Attack();

        ShowSkillDescriptionUI();
        onComplete?.Invoke();
    }

    void ShowSkillDescriptionUI()
    {
        Managers.TurnManager.BeatBarPanelBehaviour.ShowSkillDescriptionUI(description);
    }
}
