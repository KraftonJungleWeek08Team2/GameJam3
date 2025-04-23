using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "Scriptable Objects/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    [Header("스탯")]
    public string enemyName;
    public int   maxHp;
    public int   damage;
    public float moveSpeed;

    [Header("적 정보")]
    public Sprite  icon;
    public GameObject prefab;
}
