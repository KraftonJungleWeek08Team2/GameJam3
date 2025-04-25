using System.Collections.Generic;
using UnityEngine;

public class SkillBook
{
    private readonly Dictionary<CombinationType, ISkill> _skills = new Dictionary<CombinationType, ISkill>();
    
    public SkillBook()
    {
        // 스킬 등록
        // _skills[CombinationType.조합이름] = new 스킬스크립트();
    }
    
    public void TryActivateSkill(CombinationType? type)
    {
        if (type.HasValue && _skills.TryGetValue(type.Value, out var skill))
        {
            skill.Execute();
        }
    }
}
