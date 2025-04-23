using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyInfo[] _enemyInfos;
    [SerializeField] private Transform _spawnPoint;
    void Start()
    {
        //리소스 폴더안에 있는 적 정보 불러오기
        _enemyInfos = Resources.LoadAll<EnemyInfo>("KDY/EnemyInfos");
        _spawnPoint = FindObjectOfType<SpawnPoint>().transform;
    }

    void Update()
    {
        //테스트 코드
        if (Input.GetKeyDown(KeyCode.A))
        {
            // 적 소환
            Spawn(0);
        }
    }

    public void Spawn(int index)
    {
        EnemyInfo data = _enemyInfos[index];
        GameObject enemy = Instantiate(data.prefab, _spawnPoint.position, Quaternion.identity);
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.Init(data);
    }
}
