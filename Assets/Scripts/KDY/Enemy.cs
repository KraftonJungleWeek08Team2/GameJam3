using System;
using UnityEditor.Animations;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyInfo _data;
    public int hp { get; private set; }
    private Animator _animator;
    [SerializeField] float moveSpeed;


    // 스포너에서 값 넘겨줌
    public void Init(EnemyInfo data)
    {
        _data      = data;
        hp = data.maxHp;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void TakeDamage(int amount)
    {
        hp -= amount;
        _animator.Play("Hit");
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
        Debug.Log("Enemy has died.");
        _animator.Play("Death");
    }
}
