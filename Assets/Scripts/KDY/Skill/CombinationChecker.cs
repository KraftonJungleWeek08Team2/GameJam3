using UnityEngine;

public static class CombinationChecker
{
    public static CombinationType? Check(SlotInfo info)
    {
        int a = info.GetValue(0), b = info.GetValue(1), c = info.GetValue(2);

        // 777 → Jackpot 우선
        if (a == 7 && b == 7 && c == 7)
            return CombinationType.Jackpot;

        // 세 개 동일
        if (a == b && b == c)
            return CombinationType.ThreeOfAKind;

        // 연속 오름차순
        if (b == a + 1 && c == b + 1)
            return CombinationType.Sequential;

        // 모두 홀수
        if (a % 2 == 1 && b % 2 == 1 && c % 2 == 1)
            return CombinationType.AllOdd;

        // 모두 짝수
        if (a % 2 == 0 && b % 2 == 0 && c % 2 == 0)
            return CombinationType.AllEven;

        return null; // 조합없음
    }

}
