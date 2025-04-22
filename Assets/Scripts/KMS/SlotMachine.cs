using TMPro;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private int slotCount = 3; // 슬롯의 갯수 지정
    [SerializeField] private float spinSpeed = 20f; // 슬롯의 숫자 지정하기 전에 돌아가는 속도
    [SerializeField] private TMP_Text[] slotTexts; // 슬롯 텍스트를 3개로 우선 할당함돠

    private SlotInfo slotInfo;
    private int currentSlotIndex;
    private bool isSpinning; //회전하고 있음
    private int[] displayValues; // 화면에 나타나는 숫자

    /// <summary>
    /// 시작할때 한번 초기화
    /// </summary>
    void Start()
    {
        slotInfo = new SlotInfo(slotCount);
        displayValues = new int[slotCount];
        currentSlotIndex = 0;
        isSpinning = true;
    }
    /// <summary>
    /// 슬롯이 돌아가게 실행이 되며 스페이스바를 누르면 하나씩 지정이됩니당.
    /// </summary>
    void Update()
    {
        if (isSpinning)
            SpinAllUnfixedSlots();
        if (Input.GetKeyDown(KeyCode.Space))
            ConfirmCurrentSlot();
    }

    /// <summary>
    /// 1에서 9까지의 숫자를 랜덤하게 나타내는 부분
    /// </summary>
    void SpinAllUnfixedSlots()
    {
        for (int i = currentSlotIndex; i < slotCount; i++)
        {
            displayValues[i] = Random.Range(1, 10);
            slotTexts[i].text = displayValues[i].ToString();
        }
    }
    /// <summary>
    /// 스페이스바를 누르면 숫자가 하나씩 지정이 됨
    /// </summary>
    void ConfirmCurrentSlot()
    {
        int finalValue = displayValues[currentSlotIndex];
        slotInfo.SetValue(currentSlotIndex, finalValue);

        // UI에 확정값을 큰 변화없이 지정해줌 / 나중에는 슬롯 내부의 회전을 통해서 보여주도록 수정 진행
        slotTexts[currentSlotIndex].text = finalValue.ToString();

        currentSlotIndex++;
        if (currentSlotIndex >= slotCount)
        {
            isSpinning = false;
            OnAllSlotsConfirmed();
        }
    }
    /// <summary>
    /// 최종적으로 만들어졌고 전달해야할 슬롯의 숫자값
    /// </summary>
    void OnAllSlotsConfirmed()
    {
        int first = slotInfo.GetValue(0);
        int second = slotInfo.GetValue(1);
        int third = slotInfo.GetValue(2);
        Debug.Log($"{first} , {second}, {third}");
    }
}
