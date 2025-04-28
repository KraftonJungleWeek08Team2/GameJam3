using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineTutorial : MonoBehaviour
{
    [SerializeField] private ReelController[] _reels;      // 3개의 ReelController
    [SerializeField] private float _baseSpinSpeed = 750f; // 회전 속도

    
    private int _currentReel = 0;
    private bool _isSpinning; //회전하고 있음
    private Canvas _slotCanvas;
    
    Coroutine _coroutine;


    void Awake()
    {
        _slotCanvas = FindAnyObjectByType<UI_Slot_Canvas_Tutorial>().GetComponent<Canvas>();
        _reels = FindAnyObjectByType<UI_Slot_Canvas_Tutorial>().GetComponentsInChildren<ReelController>();
        if (_slotCanvas == null)
        {
            Debug.LogError("UI_Slot_Canvas not found");
            return;
        }
    }
    private void Start()
    {
        ShowSlotUI();
    }

    public void ShowSlotUI()
    {

        _currentReel = 0;
        _slotCanvas.enabled = true;
        // 모두 스핀 시작 & 콜백 구독
        for (int i = 0; i < _reels.Length; i++)
        {
            _reels[i].Init();
            _reels[i].OnReelStopped += HandleReelStopped;
            float baseSpeed = _baseSpinSpeed + (i * 150);
            _reels[i].StartSpin(baseSpeed);
        }
        _isSpinning = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.X) || Input.GetMouseButtonDown(0))
        {
            ConfirmCurrentSlot();
        }
    }

    /// <summary>
    /// 스페이스바를 누르면 0번 Reel부터 하나씩 멈추게 됩니다.
    /// </summary>
    void ConfirmCurrentSlot()
    {
        if (_currentReel < _reels.Length)
        {
            _reels[_currentReel].StopSpin();
        }
    }

    private void HandleReelStopped(int value)
    {
        // 구독해제
        _reels[_currentReel].OnReelStopped -= HandleReelStopped;
        
        // 슬롯이 멈출때까지 대기
        StartCoroutine(WaitReelsStop());
        // 값 저장
        _currentReel++;

        if (_currentReel >= _reels.Length)
        {
            // 모두 멈춤
            OnAllSlotsConfirmed();
            
        }
    }

    private void OnAllSlotsConfirmed()
    {
        if (_coroutine == null)
            _coroutine = StartCoroutine(WaitOneFrame());
    }
    
    //현재 reel 스핀이 멈출때까지 대기
    IEnumerator WaitReelsStop()
    {
        while (_reels[_currentReel].isSpinning)
        {
            yield return null;
        }
    }

    IEnumerator WaitOneFrame()
    {
        yield return new WaitForSeconds(2f);

            
        _coroutine = null;

        ShowSlotUI();
    }    


    public void HideSlotUI()
    {
        _isSpinning = false;
        _slotCanvas.enabled = false;
    }
    private void OnDisable()
    {
        for (int i = 0; i < 3; i++)
        {
            Debug.Log("해제" + i);
            _reels[i].OnReelStopped -= HandleReelStopped;
        }
    }
}
