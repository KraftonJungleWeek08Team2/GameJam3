using System;
using UnityEngine;
using UnityEngine.UI;

public class FeverTimeController : MonoBehaviour
{
    public Action OnFeverAttackEvent;
    public Action OnFeverEndEvent;

    [SerializeField] GameObject _fevertime;
    [SerializeField] float _feverTime = 4f;
    float _feverTimer = 0f;
    Slider _timerSlider;
    
    void Start()
    {
        _timerSlider = GetComponentInChildren<Slider>();
        _fevertime.SetActive(false);
    }

    public void ShowFeverTime()
    {
        _feverTimer = 0f;
        _timerSlider.value = 1f;
        _fevertime.SetActive(true);
        Managers.InputManager.OnRhythmAttackEvent += FeverAttack;
    }

    public void HideFeverTime()
    {
        _fevertime.SetActive(false);
        Managers.InputManager.OnRhythmAttackEvent -= FeverAttack;
    }

    public void UpdateFeverTimer()
    {
        _feverTimer += Time.deltaTime;
        _timerSlider.value = 1 - (_feverTimer / _feverTime);
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
