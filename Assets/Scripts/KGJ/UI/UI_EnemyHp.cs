using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyHp : MonoBehaviour
{
    Canvas _enemyHpCanvas;
    Slider _slider;

    void Start()
    {
        _enemyHpCanvas = GetComponent<Canvas>();
        _slider = GetComponentInChildren<Slider>();
    }

    public void ShowEnemyUI()
    {
        _enemyHpCanvas.enabled = true;
    }

    public void HideEnemyUI()
    {
        _enemyHpCanvas.enabled = false;
    }

    public void UpdateEnemyUI()
    {
        //_slider.value = Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().hp / Managers.TurnManager.CurrentEnemy.GetComponent<Enemy>().maxhp;
    }
}
