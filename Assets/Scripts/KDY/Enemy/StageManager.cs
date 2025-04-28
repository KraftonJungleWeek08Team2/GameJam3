using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private StageInfo _stageInfo;
    [SerializeField] private EnemySpawner _spawner;

    public int nextIndex = 0;

    private void Awake()
    {
        if (_spawner == null)
            _spawner = FindAnyObjectByType<EnemySpawner>();
    }
    
    public void SpawnNext()
    {
        if (nextIndex >= _stageInfo.spawnSequence.Count)
        {
            Debug.Log("모든 적 처치 완료!");
            return;
        }

        EnemyInfo info = _stageInfo.spawnSequence[nextIndex++];
        Enemy enemy = _spawner.Spawn(info);
        Managers.TurnManager.CurrentEnemy = enemy;
    }
}
