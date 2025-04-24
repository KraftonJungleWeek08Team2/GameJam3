using UnityEngine;

public class KnockBackState : ITurnState
{
    Rigidbody2D _rb;
    bool _isKnockback = true;
    float _knockbackTimer = 0f;
    float _knockbackDuration = 0.5f;
    float _knockbackForce = 10f;
    float _initPositionX;

    public void EnterState()
    {
        _initPositionX = Managers.TurnManager.Player.transform.position.x; // 초기 위치 저장
        Managers.CameraManager.ChangeBattleCamera(false); // 카메라를 MoveCamera로 변경
        Managers.TurnManager.Player.TakeDamage(Managers.TurnManager.CurrentEnemy.damage); // 플레이어가 적 공격력만큼의 데미지를 받음
        if (Managers.TurnManager.Player.hp > 0)
        {
            _rb = Managers.TurnManager.Player.GetComponent<Rigidbody2D>();
            _knockbackTimer = _knockbackDuration;
            _rb.AddForce(Vector2.left * _knockbackForce, ForceMode2D.Impulse); // 넉백 방향으로 힘을 가함
            _isKnockback = true;
        }
    }

    public void UpdateState()
    {
        if (!_isKnockback)
        {
            if (Managers.TurnManager.Player.transform.position.x < _initPositionX)
            {
                Managers.TurnManager.Player.Run();
                _rb.linearVelocity = new Vector2(5f, _rb.linearVelocity.y); // 오른쪽으로 이동
            }
            else
            {
                _rb.linearVelocity = Vector2.zero; // 넉백 후 속도 초기화
                Managers.TurnManager.Player.transform.position = new Vector3(-1, Managers.TurnManager.Player.transform.position.y, 0);
                Managers.TurnManager.ChangeState(new MoveState()); // 넉백이 끝나면 MoveState로 전환
            }
        }
    }

    public void FixedUpdateState()
    {
        if (_isKnockback)
        {
            _knockbackTimer -= Time.fixedDeltaTime;
            if (_knockbackTimer <= 0f)
            {
                _isKnockback = false;
                _rb.linearVelocity = Vector2.zero; // 넉백 후 속도 초기화
            }
        }
    }

    public void ExitState()
    {

    }
}
