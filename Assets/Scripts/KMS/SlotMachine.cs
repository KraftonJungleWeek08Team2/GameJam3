using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class SlotTextGroup
{
    public TMP_Text topText;
    public TMP_Text centerText;
    public TMP_Text bottomText;
}
public class SlotMachine : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private int slotCount = 3; // 슬롯의 갯수 지정
    [SerializeField] private float spinSpeed = 0.1f; // 슬롯의 숫자 돌아가는 속도 조절 가능(간격이다보니 짧을 수록 빨라집니다.)
    [SerializeField] private SlotTextGroup[] slotTextGroups; // 인스펙터에서 슬롯 개수만큼 할당
    [SerializeField] private ResultUIManager resultUIManager; // 또 다른 유아이(결과 유아이)를 키기위한 선언

    //경과 시간 추가
    private float slotTimeout = 5f; // 제한 시간
    private float slotTimer = 0f;   // 경과 시간

    public SlotInfo SlotInfo => slotInfo;
    private SlotInfo slotInfo;
    private Canvas _slotCanvas;
    private float spinTimer; // Time.deltatime 활용해서 속도 조절
    private int currentSlotIndex;
    private bool isSpinning; //회전하고 있음
    private int[,] displayValues; // [slotIndex, position(0:top,1:center,2:bottom)]

    Coroutine _coroutine;

    /// <summary>
    /// 시작할때 한번 초기화
    /// </summary>
    void Awake()
    {
        _slotCanvas = FindAnyObjectByType<UI_Slot_Canvas>().GetComponent<Canvas>();
        if (_slotCanvas == null)
        {
            Debug.LogError("UI_Slot_Canvas not found");
            return;
        }
        //ShowSlotUI();
    }

    /// <summary>
    /// 초기화 되어야할 파친코 시스템 
    /// </summary>
    public void ShowSlotUI()
    {
        slotTimer = 0f; // 슬롯 시간을 0으로 
        _slotCanvas.enabled = true;
        slotInfo = new SlotInfo(slotCount);
        displayValues = new int[slotCount, 3];
        for (int i = 0; i < slotCount; i++)
        {
            for (int j = 0; j < slotCount; j++)
            {
                displayValues[i, j] = 1; // 슬롯의 숫자 초기화
            }
        }
        currentSlotIndex = 0;
        isSpinning = true;
        Managers.InputManager.OnSlotEvent += ConfirmCurrentSlot;
        // 타이머 기능

    }
    public void HideSlotUI()
    {            
        Managers.InputManager.OnSlotEvent -= ConfirmCurrentSlot;
        _slotCanvas.enabled = false;
    }

    /// <summary>
    /// 슬롯이 spinSpeed의 속도로 돌아가며 스페이스바를 누르면 하나씩 지정이됩니당.
    /// </summary>
    void Update()
    {
        //슬롯 변화 속도 조절을 위한 타임
        if (!isSpinning) return;

        spinTimer += Time.deltaTime;

        if(spinTimer >= spinSpeed)
        {
            spinTimer = 0f;
            SpinAllUnfixedSlots();
        }
        slotTimer += Time.deltaTime;
        //남은 시간을 계산 하는거
        float timeLeft = slotTimeout - slotTimer;
        int secondsLeft = Mathf.CeilToInt(Mathf.Max(timeLeft, 0f)); // 정수로 변환
        timerText.text = secondsLeft.ToString();   // 정수로 표시

        // 남은 시간 계산 & 표시
        if (slotTimer >= slotTimeout)
        {
            ForceEndWithZeros(); // 제한시간 초과 시 강제 종료
        }
    }

    /// <summary>
    /// 1에서 9까지의 숫자를 랜덤하게 나타내는 부분
    /// </summary>
    void SpinAllUnfixedSlots()
    {
        for (int i = currentSlotIndex; i < slotCount; i++)
        {
            // 밑으로 숫자 이동: center -> bottom, top -> center, new random -> top
            displayValues[i, 2] = displayValues[i, 1];
            displayValues[i, 1] = displayValues[i, 0];
            displayValues[i, 0] = Random.Range(1, 10); // 숫자 1~9까지만 나타날거유

            // UI 업데이트
            slotTextGroups[i].topText.text = displayValues[i, 0].ToString();
            slotTextGroups[i].centerText.text = displayValues[i, 1].ToString();
            slotTextGroups[i].bottomText.text = displayValues[i, 2].ToString();
        }
    }
    /// <summary>
    /// 스페이스바를 누르면 숫자가 하나씩 지정이 됨
    /// </summary>
    void ConfirmCurrentSlot()
    {
        if (!isSpinning) return;

        int finalValue = displayValues[currentSlotIndex, 1];
        Debug.Log($"{displayValues[currentSlotIndex, 1]}, {currentSlotIndex}");
        slotInfo.SetValue(currentSlotIndex, finalValue);

        // 중앙 텍스트 강조
        //slotTextGroups[currentSlotIndex].centerText.color = Color.black;
        
        currentSlotIndex++;
        if (currentSlotIndex >= slotCount)
        {
            //디버깅 해봤을때 이 친구가 뭔가 발목이 될거 같아서 미리 표시해두는겁니다. 혹시나,,,
            isSpinning = false;
            OnAllSlotsConfirmed();
        }
    }
    /// <summary>
    /// 5초안에 안하면 0으로 반환하는 값
    /// </summary>
    void ForceEndWithZeros()
    {
        isSpinning = false;

        for (int i = currentSlotIndex; i < slotCount; i++)
        {
            slotInfo.SetValue(i, 0);
            slotTextGroups[i].centerText.text = "0";
            //slotTextGroups[i].centerText.color = Color.gray;
        }

        OnAllSlotsConfirmed();
    }
    /// <summary>
    /// 최종적으로 만들어졌고 전달해야할 슬롯의 숫자값
    /// </summary>
    void OnAllSlotsConfirmed()
    {
        if (_coroutine == null)
            StartCoroutine(WaitOneFrame());
    }

    public void HideResult()
    {
        resultUIManager.Hide();
    }

    IEnumerator WaitOneFrame()
    {
        int[] result = new int[slotCount];
        for (int i = 0; i < slotCount; i++)
            result[i] = slotInfo.GetValue(i);

        yield return new WaitForSeconds(0.1f);

        if (IsSlotSuccess())
            resultUIManager.ShowResult(result);
        Managers.TurnManager.EndSlotState();
        _coroutine = null;
    }

    bool IsSlotSuccess()
    {
        for (int i = 0; i < slotInfo.SlotCount; i++)
        {
            if (slotInfo.GetValue(i) == 0)
                return false;
        }
        return true;
    }

    private void OnDisable()
    {
        Managers.InputManager.OnSlotEvent -= ConfirmCurrentSlot;
    }
}
