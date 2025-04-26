using System;
using UnityEngine;
using UnityEngine.UI;

public class FeverTimeController : MonoBehaviour
{
    public Action OnFeverAttackEvent;
    public Action OnFeverEndEvent;

    Canvas _feverTimeUI;
    [SerializeField] float _feverTime = 2f;
    float _feverTimer = 0f;
    Slider _timerSlider;
    
    void Start()
    {
        _feverTimeUI = GetComponent<Canvas>();
        _timerSlider = GetComponentInChildren<Slider>();
        _feverTimeUI.enabled = false;
    }

    public void ShowFeverTime()
    {
        _feverTimer = 0f;
        _timerSlider.value = 1f;
        _feverTimeUI.enabled = true;
        Managers.InputManager.OnRhythmAttackEvent += FeverAttack;
    }

    public void HideFeverTime()
    {
        _feverTimeUI.enabled = false;
        Managers.InputManager.OnRhythmAttackEvent -= FeverAttack;
    }

    public void UpdateFeverTimer()
    {
        _feverTimer += Time.deltaTime;
        _timerSlider.value = _feverTime / _feverTimer;
        if (_feverTimer >= _feverTime)
        {
            OnFeverEndEvent?.Invoke();
        }
    }

    void FeverAttack()
    {
        OnFeverAttackEvent?.Invoke();
    }


    private void OnDestroy()
    {
        Managers.InputManager.OnRhythmAttackEvent -= FeverAttack;
    }
}
