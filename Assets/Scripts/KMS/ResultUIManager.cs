using TMPro;
using UnityEngine;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] numberTexts; // 3개 텍스트
    Canvas _resultUICanvas;

    private void Start()
    {
        _resultUICanvas = transform.GetChild(0).GetComponent<Canvas>();
        _resultUICanvas.enabled = false;
    }
    public void ShowResult(int[] values)
    {
        //Debug.Log("ShowResult() 호출됨");
        _resultUICanvas.enabled = true;
        for (int i = 0; i < numberTexts.Length; i++)
        {
            numberTexts[i].text = values[i].ToString();
        }
    }

    public void Hide()
    {
        _resultUICanvas.enabled = false;
    }
}
