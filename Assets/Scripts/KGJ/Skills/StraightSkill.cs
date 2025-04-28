public class StraightSkill : ISkill
{
    private string description = "Fever Time.";

    public void Execute()
    {

    }

    public void ShowSkillDescriptionUI()
    {
        Managers.TurnManager.BeatBarSystem.GetComponent<BeatBarUISystem>().ShowSkillDescriptionUI(description);
    }
}
