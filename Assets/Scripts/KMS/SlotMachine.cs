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
    [SerializeField] private int slotCount = 3; // 슬롯의 갯수 지정
    [SerializeField] private float spinSpeed = 0.1f; // 슬롯의 숫자 돌아가는 속도 조절 가능(간격이다보니 짧을 수록 빨라집니다.)
    [SerializeField] private SlotTextGroup[] slotTextGroups; // 인스펙터에서 슬롯 개수만큼 할당

    public SlotInfo SlotInfo => slotInfo;
    private SlotInfo slotInfo;
    private Canvas _slotCanvas;
    private float spinTimer; // Time.deltatime 활용해서 속도 조절
    private int currentSlotIndex;
    private bool isSpinning; //회전하고 있음
    private int[,] displayValues; // [slotIndex, position(0:top,1:center,2:bottom)]

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
        _slotCanvas.enabled = true;
        slotInfo = new SlotInfo(slotCount);
        displayValues = new int[slotCount, 3];
        currentSlotIndex = 0;
        isSpinning = true;
        Managers.InputManager.OnSlotEvent += ConfirmCurrentSlot;


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
        if (!isSpinning) return;
        // 타이머를 누적하고요
        spinTimer += Time.deltaTime;
        //0.5초의 간격이 지나면 로직을 실행함돠
        if(spinTimer >= spinSpeed)
        {
            spinTimer = 0f;
            SpinAllUnfixedSlots();
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
            displayValues[i, 0] = Random.Range(1, 10);

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
        //Debug.Log($"{displayValues[currentSlotIndex, 1]}, {currentSlotIndex}");
        slotInfo.SetValue(currentSlotIndex, finalValue);

        // 중앙 텍스트 강조
        slotTextGroups[currentSlotIndex].centerText.color = Color.black;
        
        currentSlotIndex++;
        if (currentSlotIndex >= slotCount)
        {
            //디버깅 해봤을때 이 친구가 뭔가 발목이 될거 같아서 미리 표시해두는겁니다. 혹시나,,,
            isSpinning = false;
            OnAllSlotsConfirmed();
        }
    }

    /// <summary>
    /// 최종적으로 만들어졌고 전달해야할 슬롯의 숫자값
    /// </summary>
    void OnAllSlotsConfirmed()
    {
        //콘솔창에 확인용으로 잠깐 쓴겁니다. 지우셔도 되요.
        int first = slotInfo.GetValue(0);
        int second = slotInfo.GetValue(1);
        int third = slotInfo.GetValue(2);

        //Debug.Log($"{first} , {second}, {third}");
        StartCoroutine(WaitOneFrame());
    }

    IEnumerator WaitOneFrame()
    {

        yield return new WaitForSecondsRealtime(0.1f);
        Managers.TurnManager.EndSlotState();
    }
}
