using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private int maxHp = 100; // 최대 체력
    public int hp{ get; private set; } // 현재 체력

    [Tooltip("애니메이터 컴포넌트")]
    [SerializeField] private Animator _animator;

    int attackIndex = 0; // 공격 인덱스

    public Action<int> OnPlayerDamageEvent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        hp = maxHp;
        FakeRun();
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

    public void Attack()
    {
        _animator.Play($"Player_v2_Attack{attackIndex + 1}");
        attackIndex = (attackIndex + 1) % 3;
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
        _animator.SetTrigger("TakeDamage");
        OnPlayerDamageEvent?.Invoke(amount);
        IsDie();
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
