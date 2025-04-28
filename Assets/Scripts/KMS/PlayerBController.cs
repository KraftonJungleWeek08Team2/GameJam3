using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerBController : MonoBehaviour
{
    private Animator _anim;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        // Animator Controller의 Any State나 Entry에서
        // 다른 상태로 전이(Transition) 설정이 없는지 확인하세요.
    }

    void Start()
    {
        // 해당 레이어(0)에 있는 "Player_Run" 스테이트로 즉시 전환
        _anim.Play("Player_Run", 0, 0f);
    }
}
