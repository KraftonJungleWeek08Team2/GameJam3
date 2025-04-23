using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] _enemyInfos;
    [SerializeField] private Transform _spawnPoint;

    void Start()
    {
        _enemyInfos = Resources.LoadAll<EnemyInfo>("KDY/EnemyInfos");
        _spawnPoint = FindObjectOfType<SpawnPoint>().transform;
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Spawn(_spawnPoint);
        }
    }

    public void Spawn(Transform spawnPoint)
    {
        int idx = Random.Range(0, _enemyInfos.Length);
        var data = _enemyInfos[idx];
        var go = Instantiate(data.prefab, spawnPoint.position, Quaternion.identity);
        go.GetComponent<Enemy>().Init(data);
    }
}