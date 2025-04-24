using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyHp : MonoBehaviour
{
    Canvas _enemyHpCanvas;
    Slider _slider;
    RectTransform _rectTransform;

    void Start()
    {
        _enemyHpCanvas = GetComponent<Canvas>();
        _slider = GetComponentInChildren<Slider>();
        _enemyHpCanvas.enabled = false;
    }

    public void ShowEnemyUI()
    {
        _slider.value = (float)Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().hp / (float)Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().maxHp;
        _enemyHpCanvas.enabled = true;
    }

    public void HideEnemyUI()
    {
        _enemyHpCanvas.enabled = false;
    }

    public void UpdateEnemyUI()
    {
        float hp = Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().hp;
        
        if ((float)Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().hp <= 0)
        {
            hp = 0;
        }
        else
        {
            hp = (float)Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().hp / (float)Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().maxHp;
        }
        Debug.Log("[KGJ]" + hp);
        _slider.value = hp;
    }
}
