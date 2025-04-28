using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [SerializeField] private StageInfo _stageInfo;
    [SerializeField] private EnemySpawner _spawner;
    [SerializeField] float _fadeDuration = 4f;
    Image _fadeoutImage;
    public int _sceneIndex;
    Color _transparentColor = new Color(0f, 0f, 0f, 0f);

    public int nextIndex = 0;
    public int stageIndex = 0;

    private void Awake()
    {
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
        _fadeoutImage = FindAnyObjectByType<UI_BlackImage>().GetComponent<Image>();
        stageIndex = _stageInfo.spawnSequence.Count;

        if (_sceneIndex == 5) // MainScene_Noraml이라면 투명으로 시작
        {
            _fadeoutImage.color = _transparentColor;
        }
        else if (_sceneIndex == 6) // MainScene_Hard라면 검은색으로 시작
        {
            _fadeoutImage.color = Color.black;
            StartCoroutine(FadeIn());
        }

        if (_spawner == null)
            _spawner = FindAnyObjectByType<EnemySpawner>();
    }
    
    public void SpawnNext()
    {
        if (_spawner == null)
            _spawner = FindAnyObjectByType<EnemySpawner>();

        if (nextIndex >= _stageInfo.spawnSequence.Count)
        {
            // 스테이지 클리어
            Managers.GameManager.StageClear();
            StartCoroutine(FadeOutAndLoadScene());
            return;
        }

        EnemyInfo info = _stageInfo.spawnSequence[nextIndex++];
        Enemy enemy = _spawner.Spawn(info);
        Managers.TurnManager.CurrentEnemy = enemy;
    }

    IEnumerator FadeOutAndLoadScene()
    {
        // TODO : 현우님 여기에 BGM 페이드아웃 넣어주세요~~
        Color color = _fadeoutImage.color;
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(elapsed / _fadeDuration);
            _fadeoutImage.color = color;
            yield return null;
        }

        // 페이드 완료 후 다음 씬 로드
        SceneManager.LoadScene(_sceneIndex + 1);
    }

    IEnumerator FadeIn()
    {
        Color color = _fadeoutImage.color;
        float elapsed = 0f;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Clamp01(1f- (elapsed / _fadeDuration));
            _fadeoutImage.color = color;
            yield return null;
        }
    }
}
