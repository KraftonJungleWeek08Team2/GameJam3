using System.Collections.Generic;

public class SkillBook
{
    private readonly Dictionary<CombinationType, ISkill> _skills = new Dictionary<CombinationType, ISkill>();
    
    public SkillBook()
    {
        // 스킬 등록
        // _skills[CombinationType.조합이름] = new 스킬스크립트();
        _skills[CombinationType.ThreeOfAKindEven] = new Heal(3); 
        _skills[CombinationType.ThreeOfAKindOdd] = new ThreeOddSkill(25);
        _skills[CombinationType.AllOdd] = new OddSkill(6);
        _skills[CombinationType.AllEven] = new EvenSkill(6);
        _skills[CombinationType.Jackpot] = new JackpotSkill();
        _skills[CombinationType.Sequential] = new StraightSkill();
    }

    public void TryShowSkillDescriptionUI(CombinationType? type)
    {
        if (type.HasValue && _skills.TryGetValue(type.Value, out var skill))
        {
            skill.ShowSkillDescriptionUI();
        }
    }

    public void TryActivateSkill(CombinationType? type)
    {
        if (type.HasValue && _skills.TryGetValue(type.Value, out var skill))
        {
            skill.Execute();
        }
    }
}
