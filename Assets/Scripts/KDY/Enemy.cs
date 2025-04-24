using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyInfo _data;

    public int maxHp;
    public int hp { get; private set; }
    public int damage { get; private set; }
    private Animator _animator;
    private float _moveSpeed;
    
    public bool isMoving;


    // 스포너에서 값 넘겨줌
    public void Init(EnemyInfo data)
    {
        _data      = data;
        hp = data.maxHp;
        damage = data.damage;
        _moveSpeed = data.moveSpeed;
        isMoving   = true;
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
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
        isMoving = true;
        Debug.Log("Enemy has died.");
        _animator.Play("Death");
    }
}
