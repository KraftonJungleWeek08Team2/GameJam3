using UnityEngine;

public class BeatBarBehaviour : MonoBehaviour
{
    
    [SerializeField] private int beatMargin = 2; // 노트가 중앙에 도달하는 비트 수
    [SerializeField] private RectTransform leftCircle; // 왼쪽 원
    [SerializeField] private RectTransform rightCircle; // 오른쪽 원
    [SerializeField] private float initialDistance = 200f; // 초기 거리 (픽셀, 양쪽 원의 시작 위치)
    [SerializeField] private GameObject disappearEffectObject; //사라질 때 보일 파티클. 프리팹.
    
    private float smallingTime; // 애니메이션 지속 시간
    private float elapsedTime;
    private Vector2 leftInitialPos;
    private Vector2 rightInitialPos;
    public string noteIndex;

    void Start()
    {
        smallingTime = MusicManager.Instance.beatInterval * beatMargin;
        if (leftCircle == null || rightCircle == null)
        {
            Debug.LogError("BeatBarBehaviour: LeftCircle or RightCircle not assigned!");
            return;
        }

        // 초기 위치 설정
        leftInitialPos = new Vector2(-initialDistance, 0f);
        rightInitialPos = new Vector2(initialDistance, 0f);
        leftCircle.localPosition = leftInitialPos;
        rightCircle.localPosition = rightInitialPos;

        elapsedTime = 0f;
    }

    void Update()
    {
        if (elapsedTime < smallingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / smallingTime; // 진행률 (0 to 1)

            // 양쪽 원을 중앙(x=0)으로 이동
            leftCircle.localPosition = Vector2.Lerp(leftInitialPos, new Vector2(-10, 0), t);
            rightCircle.localPosition = Vector2.Lerp(rightInitialPos, new Vector2(10, 0), t);
        }
        else
        {
            // 애니메이션 완료 시 정확히 중앙에 위치
            leftCircle.localPosition = Vector2.zero;
            rightCircle.localPosition = Vector2.zero;

            GetComponent<CanvasGroup>().alpha = 0;
        }
    }

    [SerializeField] private GameObject perfectNoteEffect;
    [SerializeField] private GameObject goodNoteEffect;
    [SerializeField] private GameObject missNoteEffect;

    public void Disappear(AccuracyType accuracyType)
    {
        GetComponent<CanvasGroup>().alpha = 0;

        if(accuracyType == AccuracyType.Perfect)
        {
            Instantiate(perfectNoteEffect, transform);
        }
        if(accuracyType == AccuracyType.Good)
        {
            Instantiate(goodNoteEffect, transform);
        }
        if(accuracyType == AccuracyType.Miss)
        {
            Instantiate(missNoteEffect, transform);
        }
    }
}