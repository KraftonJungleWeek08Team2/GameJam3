using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachineV2 : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private ReelController[] _reels;      // 3개의 ReelController
    [SerializeField] private float _baseSpinSpeed = 750f; // 회전 속도
    [SerializeField] private ResultUIManager _resultUIManager; // 결과 UI
    [SerializeField] private Slider _timerSlider; // 타이머 슬라이더 UI

    //경과 시간 추가
    private float _maxSlotTime = 5f;
    private float _slotTimeout = 5f; // 제한 시간
    private float _slotTimer = 0f;   // 경과 시간
    
    private int _currentReel = 0;
    public SlotInfo SlotInfo => _slotInfo;
    private SlotInfo _slotInfo;
    private bool _isSpinning; //회전하고 있음
    private Canvas _slotCanvas;
    
    Coroutine _coroutine;

    void Awake()
    {
        _slotCanvas = FindAnyObjectByType<UI_Slot_Canvas_v2>().GetComponent<Canvas>();
        _resultUIManager = FindAnyObjectByType<ResultUIManager>();
        _timerText = FindAnyObjectByType<TimerText>().GetComponent<TMP_Text>();
        _reels = FindAnyObjectByType<UI_Slot_Canvas_v2>().GetComponentsInChildren<ReelController>();
        _timerSlider = FindAnyObjectByType<Slider>();
        if (_slotCanvas == null)
        {
            Debug.LogError("UI_Slot_Canvas not found");
            return;
        }
        //ShowSlotUI();
    }

    public void ShowSlotUI(bool isOneMore)
    {
        _slotTimer = 0f; // 슬롯 타이머 초기화
        _timerSlider.value = 1f; // 슬라이더 초기화

        if (isOneMore)
        {
            if (_slotTimeout > 1f)
            { 
                _slotTimeout -= 1f;
            }
        }
        else
        {
            _slotTimeout = _maxSlotTime; // 슬롯 제한시간 초기화
        }
        _currentReel = 0;
        _slotCanvas.enabled = true;
        _slotInfo = new SlotInfo(_reels.Length);

        // 모두 스핀 시작 & 콜백 구독
        for (int i = 0; i < _reels.Length; i++)
        {
            _reels[i].OnReelStopped += HandleReelStopped;
            _reels[i].StartSpin(_baseSpinSpeed);
        }
        _isSpinning = true;
        Managers.InputManager.OnSlotEvent += ConfirmCurrentSlot;
    }

    void Update()
    {
        //슬롯 변화 속도 조절을 위한 타임
        if (!_isSpinning) return;

        // 타이머 작동
        _slotTimer += Time.deltaTime; // 경과 시간 증가

        //남은 시간을 계산 하는거
        float timeLeft = _slotTimeout - _slotTimer;
        int secondsLeft = Mathf.CeilToInt(Mathf.Max(timeLeft, 0f)); // 정수로 변환
        _timerText.text = secondsLeft.ToString();   // 정수로 표시
        _timerSlider.value = timeLeft / _slotTimeout;   // 슬라이더 값 설정

        // 남은 시간 계산 & 표시
        if (_slotTimer >= _slotTimeout)
        {
            ForceEndWithZeros(); // 제한시간 초과 시 강제 종료
        }
    }
    /// <summary>
    /// 5초안에 안하면 0으로 반환하는 값
    /// </summary>
    void ForceEndWithZeros()
    {
        _isSpinning = false;

        for (int i = _currentReel; i < _reels.Length; i++)
        {
            _slotInfo.SetValue(i, 0);
            _reels[i].StopSpin();
            TMP_Text[] tmpText = _reels[i].GetComponentsInChildren<TMP_Text>();
            for (int j = 0; j < tmpText.Length; j++)
            {
                tmpText[j].text = "0";
            }
            //slotTextGroups[i].centerText.color = Color.gray;
        }

        OnAllSlotsConfirmed();
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
        if (_coroutine == null)
        {
            
            StartCoroutine(WaitReelsStop());
        }
        // 값 저장
        _slotInfo.SetValue(_currentReel, value);
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
            StartCoroutine(WaitOneFrame());
    }

    public void HideResult()
    {
        _resultUIManager.Hide();
    }
    
    //현재 reel 스핀이 멈출때까지 대기
    IEnumerator WaitReelsStop()
    {
        while (_reels[_currentReel].isSpinning)
        {
            yield return null;
        }
        SoundManager.Instance.PlaySlotLoadSound();
        _coroutine = null;
    }

    IEnumerator WaitOneFrame()
    {
        int[] result = new int[_reels.Length];
        for (int i = 0; i < _reels.Length; i++)
        {
            result[i] = _slotInfo.GetValue(i);
        }
        
        yield return new WaitForSeconds(0.3f);

        if (IsSlotSuccess())
        {
            _resultUIManager.ShowResult(result);
        }
        
        
        Managers.TurnManager.EndSlotState();
        _coroutine = null;
    }    
    private bool IsSlotSuccess()
    {
        for (int i = 0; i < _slotInfo.SlotCount; i++)
        {
            if (_slotInfo.GetValue(i) == 0)
                return false;
        }
        return true;
    }

    public void HideSlotUI()
    {
        _isSpinning = false;
        Managers.InputManager.OnSlotEvent -= ConfirmCurrentSlot;
        _slotCanvas.enabled = false;
    }
    private void OnDisable()
    {
        Managers.InputManager.OnSlotEvent -= ConfirmCurrentSlot;
    }
}
