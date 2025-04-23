using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class ReelController : MonoBehaviour
{
    [Header("UI 세팅")]
    [SerializeField] private RectTransform content;      // 심볼들을 담고 있는 부모
    [SerializeField] private GameObject symbolPrefab;    // 숫자 TMP_Text 프리팹

    [Header("룰렛 설정")]
    [SerializeField] private int symbolCount = 9;        // 1~9
    [SerializeField] private float symbolSpacing = 100f; // 각 심볼 높이

    private float totalHeight;                          // symbolCount * symbolSpacing
    private List<RectTransform> symbols = new();        // 생성된 심볼들
    public bool isSpinning;
    private float spinSpeed;
    
    private Coroutine _spinCoroutine;

    void Start()
    {
        // 1) 전체 높이 계산
        totalHeight = symbolCount * symbolSpacing;    // 9 * 100 = 900

        // 2) “중앙에 올 인덱스” 랜덤 결정 (0~8)
        int rndIdx = Random.Range(0, symbolCount);

        // 3) 중앙에 위치할 인덱스 위치까지의 오프셋(y)을 계산
        //    centerOffset = (symbolCount-1)/2 * symbolSpacing
        float centerOffset = (symbolCount - 1) * 0.5f * symbolSpacing;
        //    초기 오프셋 = rndIdx*spacing - centerOffset
        float initialOffsetY = rndIdx * symbolSpacing - centerOffset;

        // 4) 심볼 두 바퀴(18개) 배치
        for (int i = 0; i < symbolCount * 2; i++)
        {
            var go = Instantiate(symbolPrefab, content);
            var rt = go.GetComponent<RectTransform>();

            // 1~9 순차 텍스트
            int value = (i % symbolCount) + 1;
            go.GetComponentInChildren<TMP_Text>().text = value.ToString();

            // y 위치 = centerOffset - i*spacing + initialOffsetY
            float y = centerOffset - i * symbolSpacing + initialOffsetY;
            rt.anchoredPosition = new Vector2(0, y);

            symbols.Add(rt);
        }
    }

    public void StartSpin(float speed)
    {
        spinSpeed = speed;
        if (isSpinning) return;               // 이미 스핀 중이면 중복 실행 방지
        isSpinning = true;

        // 이전에 남아있는 코루틴이 있으면 정리
        if (_spinCoroutine != null)
            StopCoroutine(_spinCoroutine);

        _spinCoroutine = StartCoroutine(Spin());
    }

    private IEnumerator Spin()
    {
        while (isSpinning)
        {
            float delta = spinSpeed * Time.deltaTime;
            foreach (var rt in symbols)
            {
                float newY = rt.anchoredPosition.y - delta;
                if (newY < -totalHeight/2)
                    newY += totalHeight;
                rt.anchoredPosition = new Vector2(0, newY);
            }
            yield return null;
        }
        _spinCoroutine = null; // 코루틴 종료 표시
    }

    public void StopSpin()
    {
        // 감속 코루틴을 실행하기 전에 곧바로 isSpinning = false 해버리면
        // Spin() 내부의 while 루프가 종료되면서 _spinCoroutine도 끝납니다.
        isSpinning = false;                   
        // 필요하다면 StopCoroutine(_spinCoroutine); _spinCoroutine = null;
        StartCoroutine(StopAndSnap());
    }

    private IEnumerator StopAndSnap()
    {
        // 1) 감속
        float t = 0f, duration = 0.2f, startSpeed = spinSpeed;
        while (t < duration)
        {
            t += Time.deltaTime;
            spinSpeed = Mathf.Lerp(startSpeed, 0f, t / duration);
            yield return null;
        }
        isSpinning = false;

        // 2) 스냅: 각 심볼을 가장 가까운 그리드 위치로 보정
        foreach (var rt in symbols)
        {
            float y = rt.anchoredPosition.y;
            float snappedY = Mathf.Round(y / symbolSpacing) * symbolSpacing;
            rt.anchoredPosition = new Vector2(0, snappedY);
        }

        // 3) 중앙(0 위치)에 가까운 심볼 찾기
        RectTransform best = null;
        float bestDist = float.MaxValue;
        foreach (var rt in symbols)
        {
            float dist = Mathf.Abs(rt.anchoredPosition.y);
            if (dist < bestDist)
            {
                bestDist = dist;
                best = rt;
            }
        }

        // 4) 텍스트 값 읽어서 콜백
        int result = int.Parse(best.GetComponentInChildren<TMP_Text>().text);
        OnReelStopped?.Invoke(result);
    }

    public event Action<int> OnReelStopped;
}