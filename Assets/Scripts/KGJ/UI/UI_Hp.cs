using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Hp : MonoBehaviour
{
    Canvas _canvas;
    GameObject[] _hpRed = new GameObject[10];
    Player _player;
    int _hpIdx;

    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _canvas.enabled = true;
        for (int i = 0; i < _hpRed.Length; i++)
        {
            _hpRed[i] = transform.GetChild(0).GetChild(1).GetChild(i).gameObject;
        }
        _hpIdx = _hpRed.Length - 1;
        _player = FindAnyObjectByType<Player>();
        _player.OnPlayerDamageEvent += UpdateHp;
    }

    void UpdateHp(int damage)
    {
        for (int i = 0; i < damage; i++)
        {
            if (_hpIdx < 0) break;
            StartCoroutine(ShakeRectTransform(_hpRed[_hpIdx--], 0.5f, 10f));
        }
    }

    IEnumerator ShakeRectTransform(GameObject uiObject, float duration, float magnitude)
    {
        uiObject.GetComponent<Image>().color = new Color(159f / 255f, 159f / 255f, 159f / 255f);
        RectTransform target = transform.GetChild(0).GetComponent<RectTransform>();
        Vector3 originalPos = target.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            target.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        uiObject.SetActive(false);
    }

    void OnDestroy()
    {
        _player.OnPlayerDamageEvent -= UpdateHp;
    }
}
