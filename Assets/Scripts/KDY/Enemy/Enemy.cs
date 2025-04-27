using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int maxHp { get; private set; }
    public int hp { get; private set; }
    public int damage { get; private set; }
    private Animator _animator;
    private float _moveSpeed;
    
    public bool isMoving;

    UI_EnemyHp _enemyHpUI;

    // 스포너에서 값 넘겨줌
    public void Init(int enemyMaxHp, int enemyDamage, float enemyMoveSpeed)
    {
        hp = enemyMaxHp;
        maxHp = hp;
        damage = enemyDamage;
        _moveSpeed = enemyMoveSpeed;
        isMoving   = true;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyHpUI = FindAnyObjectByType<UI_EnemyHp>();
    }
    private void Update()
    {
        if (isMoving)
            Move();

        if (transform.position.x < -20)
            Destroy(gameObject);
    }

    private void Move()
    {
        transform.Translate(Vector3.left * (_moveSpeed * Time.deltaTime));
    }
    
    public void TakeDamage(int amount)
    {
        hp -= amount;
        _animator.Play("Hit");
        _enemyHpUI.UpdateEnemyUI();
    }

    public void PlayAttack()
    {
        _animator.Play("Attack");
    }
    
    public void IsDie()
    {
        if (hp <= 0)
        {
            Die();
        }
    }

    public void PlayDeath()
    {
        _animator.Play("Death");
    }

    public void Die()
    {
        isMoving = true;
        Debug.Log("Enemy has died.");
    }
}
