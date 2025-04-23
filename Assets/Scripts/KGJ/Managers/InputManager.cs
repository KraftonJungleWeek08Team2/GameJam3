using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager
{
    PlayerInputSystem_Action _playerInputSystem;
    PlayerInput _playerInput;

    InputAction _slotAction;
    InputAction _rhythmAttackAction;

    public Action OnSlotEvent;
    public Action OnRhythmAttackEvent;

    public void Init()
    {
        _playerInputSystem = new PlayerInputSystem_Action();
        _playerInput = GameObject.FindAnyObjectByType<PlayerInput>();
        _playerInput.neverAutoSwitchControlSchemes = true;

        _slotAction = _playerInputSystem.Slot.Confirm;
        _rhythmAttackAction = _playerInputSystem.RhythmAttack.Attack;

        _playerInputSystem.Enable();
    }

    public void SlotEnable(bool isEnable)
    {
        if (isEnable)
        {
            _playerInputSystem.Slot.Enable();
            _slotAction.performed += OnSlotInput;
        }
        else
        {
            _playerInputSystem.Slot.Disable();
            _slotAction.performed -= OnSlotInput;
        }
    }

    public void RhythmAttackEnable(bool isEnable)
    {
        if (isEnable)
        {
            _playerInputSystem.RhythmAttack.Enable();
            _rhythmAttackAction.performed += OnRhythmAttackInput;
        }
        else
        {
            _playerInputSystem.RhythmAttack.Disable();
            _rhythmAttackAction.performed -= OnRhythmAttackInput;
        }
            
    }

    void OnSlotInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnSlotEvent?.Invoke();
        }
    }

    void OnRhythmAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnRhythmAttackEvent?.Invoke();
        }
    }

    public void Clear()
    {
        _slotAction.performed -= OnSlotInput;
        _rhythmAttackAction.performed -= OnRhythmAttackInput;
        _playerInputSystem.Slot.Disable();
        _playerInputSystem.RhythmAttack.Disable();
        _playerInputSystem.Disable();
    }
}
