using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] _enemyInfos;
    [SerializeField] private Transform _spawnPoint;

    private void Awake()
    {
        _enemyInfos = Resources.LoadAll<EnemyInfo>("KDY/EnemyInfos");
        _spawnPoint = FindAnyObjectByType<SpawnPoint>().transform;
        Managers.TurnManager.CurrentEnemy = Spawn();
    }
    void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Spawn();
        }
    }

    public Enemy Spawn()
    {
        int idx = Random.Range(0, _enemyInfos.Length);
        EnemyInfo data = _enemyInfos[idx];
        GameObject go = Instantiate(data.prefab, _spawnPoint.position, Quaternion.identity);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.Init(data);

        return enemy;
    }
}