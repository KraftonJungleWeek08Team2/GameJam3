using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private int maxHp = 100; // 최대 체력
    public int hp{ get; private set; } // 현재 체력
    private Rigidbody2D _rb;
    [Tooltip("이동 상태")]
    [SerializeField] private bool _isMoving;
    [Header("넉백")]
    [Tooltip("넉백 상태")]
    [SerializeField] private bool _isKnockback;
    [Tooltip("넉백 힘")]
    [SerializeField] private float _knockbackForce = 10f;
    [Tooltip("넉백 지속 시간")]
    [SerializeField] private float _knockbackDuration = 0.5f;
    [Tooltip("넉백 현재 시간")]
    [SerializeField] private float _knockbackTimer = 0f;
    [Tooltip("애니메이터 컴포넌트")]
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        hp = maxHp;
        Run();
    }

    private void Update()
    {
        //애니메이션 테스트
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Attack1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Attack2();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Attack3();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            TakeDamage(10);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Die();
        }
    }
    
    private void FixedUpdate()
    {
        // 이동
        // 넉백
        if (_isKnockback)
        {
            _knockbackTimer -= Time.fixedDeltaTime;
            if (_knockbackTimer <= 0f)
            {
                _isKnockback = false;
                _rb.linearVelocity = Vector2.zero; // 넉백 후 속도 초기화
            }
        }
        else
        {
            // 이동 로직
            Run();
        }
    }
    public void Idle()
    {
        _animator.SetBool("IsRunning", false);
    }

    public void Run()
    {
        if (!_isKnockback)
        {
            _animator.SetBool("IsRunning", true);
            _isMoving = true;
            _rb.linearVelocity = new Vector2(5f, _rb.linearVelocity.y); // 오른쪽으로 이동
        }
    }
    
    public void Attack1()
    {
        _animator.SetTrigger("Attack1");
    }
    public void Attack2()
    {
        _animator.SetTrigger("Attack2");
    }
    public void Attack3()
    {
        _animator.SetTrigger("Attack3");
    }

    public void TakeDamage(int amount)
    {
        //넉백
        if (!_isKnockback)
        {
            hp -= amount;
            // 넉백 효과 적용
            _rb = GetComponent<Rigidbody2D>();
            _rb.linearVelocity = Vector2.zero; // 현재 속도를 초기화
            _rb.AddForce(Vector2.left * _knockbackForce, ForceMode2D.Impulse); // 넉백 방향으로 힘을 가함
 
            // 넉백 상태 설정 및 타이머 초기화
            _isKnockback = true;
            _knockbackTimer = _knockbackDuration;
            
            // 피격 애니메이션 실행
            _animator.SetTrigger("TakeDamage");
        }
    }
    
    public void IsDie()
    {
        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player has died.");
        _animator.SetTrigger("Death");
    }
}
