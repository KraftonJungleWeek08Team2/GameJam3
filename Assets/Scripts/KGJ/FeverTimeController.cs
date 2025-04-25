using System;
using UnityEngine;

public class FeverTimeController : MonoBehaviour
{
    public Action OnFeverAttackEvent;
    public Action OnFeverEndEvent;

    Canvas _feverTimeUI;
    [SerializeField] float _feverTime = 2f;
    float _feverTimer = 0f;
    
    void Start()
    {
        _feverTimeUI = GetComponent<Canvas>();
        _feverTimeUI.enabled = false;
    }

    public void ShowFeverTime()
    {
        _feverTimer = 0f;
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
