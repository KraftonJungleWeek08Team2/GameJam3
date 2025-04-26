using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyHp : MonoBehaviour
{
    Canvas _enemyHpCanvas;
    Slider _slider;
    RectTransform _rectTransform;
    float _maxhp;

    void Start()
    {
        _enemyHpCanvas = GetComponent<Canvas>();
        _slider = GetComponentInChildren<Slider>();
        _enemyHpCanvas.enabled = false;
    }

    public void ShowEnemyUI()
    {
        _maxhp = (float)Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().maxHp;
        _slider.value = (float)Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().hp / _maxhp;
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
            hp = (float)Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().hp / _maxhp;
        }
        _slider.value = hp;
    }
}
