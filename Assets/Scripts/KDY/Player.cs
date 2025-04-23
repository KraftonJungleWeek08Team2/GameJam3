using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private int maxHp = 100; // 최대 체력
    public int hp{ get; private set; } // 현재 체력

    [Tooltip("애니메이터 컴포넌트")]
    [SerializeField] private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        hp = maxHp;
        FakeRun();
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

    public void Idle()
    {
        _animator.SetBool("IsRunning", false);
    }

    public void FakeRun()
    {
        _animator.SetBool("IsRunning", true);
    }

    public void Run()
    {
        _animator.SetBool("IsRunning", true);
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
        hp -= amount;
        _animator.SetTrigger("TakeDamage");
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
