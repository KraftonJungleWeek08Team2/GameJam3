using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageInfo", menuName = "Scriptable Objects/StageInfo")]
public class StageInfo : ScriptableObject
{
    public int stageNumber;
    public List<EnemyInfo> spawnSequence = new List<EnemyInfo>();
}
