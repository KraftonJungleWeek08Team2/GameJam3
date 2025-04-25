using System;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    private int maxHp = 10; // 최대 체력
    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        { 
            _hp = value;
            OnPlayerHpChangeEvent?.Invoke(_hp);
        }
    } // 현재 체력
    private int _hp;

    [Tooltip("애니메이터 컴포넌트")]
    [SerializeField] private Animator _animator;

    int attackIndex = 0; // 공격 인덱스

    public Action<int> OnPlayerHpChangeEvent;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _hp = maxHp;
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

    public void TakeHeal(int heal)
    {
        Hp = Mathf.Min(Hp + heal, maxHp);
    }

    public void TakeDamage(int amount)
    {
        Hp = Mathf.Max(Hp - amount, 0);
        if (Hp > 0)
        {
            _animator.SetTrigger("TakeDamage");
        }
        IsDie();
    }
    
    public void IsDie()
    {
        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Player has died.");
        _animator.Play("Player_v2_Death");
        Managers.GameManager.GameOver();
    }
}
