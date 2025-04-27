using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    private void Awake()
    {
        _spawnPoint = FindAnyObjectByType<SpawnPoint>().transform;
    }

    public Enemy Spawn(EnemyInfo info)
    {
        GameObject go = Instantiate(info.prefab, _spawnPoint.position, Quaternion.identity);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.Init(info.maxHp, info.damage, info.moveSpeed);

        return enemy;
    }
}