using TMPro;
using UnityEngine;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] TMP_Text _progressText;
    StageManager _stageManager;

    private void Start()
    {
        _stageManager = FindAnyObjectByType<StageManager>();
    }

    public void UpdateProgressText()
    {
        _progressText.text = $"Stage {_stageManager._sceneIndex - 4} : {_stageManager.nextIndex} / {_stageManager.stageIndex}";
    }
}
