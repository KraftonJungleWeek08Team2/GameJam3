using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_HpBlock : MonoBehaviour
{
    public int hpIdx;
    Image _hpImage;
    RectTransform _rectTransform;
    Color _initColor;

    void Start()
    {
        _hpImage = GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();
        _initColor = _hpImage.color;
        Managers.TurnManager.Player.OnPlayerHpChangeEvent += ChangeHp;
    }

    void ChangeHp(int hp)
    {
        // hp 10일때 hpIdx 9 enabled true로
        // hp 9일때 hpIdx 9 enabled false
        // hp 8일 때 hpIdx 9,8 enabled false
        if (hpIdx >= hp)
        {
             StartCoroutine(ShakeRectTransform(0.5f, 5f));
        }
        else
        {
            _hpImage.color = _initColor;
            _hpImage.enabled = true;
        }
    }
 
    IEnumerator ShakeRectTransform(float duration, float magnitude)
    {
        _hpImage.color = new Color(159f / 255f, 159f / 255f, 159f / 255f);
        Vector3 originalPos = _rectTransform.anchoredPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * magnitude;
            float offsetY = Random.Range(-1f, 1f) * magnitude;

            _rectTransform.anchoredPosition = originalPos + new Vector3(offsetX, offsetY, 0);

            elapsed += Time.deltaTime;
            yield return null;
        }

        _rectTransform.anchoredPosition = originalPos;
        _hpImage.enabled = false;
    }

    private void OnDestroy()
    {
        Managers.TurnManager.Player.OnPlayerHpChangeEvent -= ChangeHp;
    }
}
