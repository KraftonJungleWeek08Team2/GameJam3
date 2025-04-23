using TMPro;
using UnityEngine;

public class ResultUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] numberTexts; // 3개 텍스트

    public void ShowResult(int[] values)
    {
        //Debug.Log("ShowResult() 호출됨");
        gameObject.SetActive(true);
        for (int i = 0; i < numberTexts.Length; i++)
        {
            numberTexts[i].text = values[i].ToString();
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
