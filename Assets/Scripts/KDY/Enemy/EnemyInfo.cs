using UnityEngine;

[System.Serializable]
public class EnemyInfo
{
    [Tooltip("스폰할 적 프리팹")]
    public GameObject prefab;

    [Tooltip("적의 종류")] 
    public EnemyType enemyType;

    [Header("스탯")]
    [Tooltip("최대 체력")]
    public int maxHp;
    [Tooltip("데미지")]
    public int damage;
    [Tooltip("이동속도")]
    public float moveSpeed;
}