using UnityEngine;


public class SkillManager : MonoBehaviour
{
    [Header("777, Attack, HEAL / SKILL")]
    [SerializeField] private Canvas _sevenPrefab;
    [SerializeField] private Canvas _healPrefab;
    [SerializeField] private Canvas _attackPrefab;

    private void Start()
    {
        _sevenPrefab.gameObject.SetActive(false);
        _healPrefab.gameObject.SetActive(false);
        _attackPrefab.gameObject.SetActive(false);
    }

    /// <summary>
    /// 프리팹 키는 것들
    /// </summary>
    public void SevenSkill()
    {
        _sevenPrefab.gameObject.SetActive(true);
    }
    public void HealSkill()
    {
        _healPrefab.gameObject.SetActive(true);
    }
    public void AttackSkill()
    {
        _attackPrefab.gameObject.SetActive(true);
    }
    /// <summary>
    /// 프리팹 끄는 것들
    /// </summary>
    public void SevenSkillFalse()
    {
        _sevenPrefab.gameObject.SetActive(false);
    }
    public void HealSkillFalse()
    {
        _healPrefab.gameObject.SetActive(false);
    }
    public void AttackSkillFalse()
    {
        _attackPrefab.gameObject.SetActive(false);
    }
}

