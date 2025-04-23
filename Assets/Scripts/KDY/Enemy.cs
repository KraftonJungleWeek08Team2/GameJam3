using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] float moveSpeed;

    public int hp { get; set; }

    public void TakeDamage(int amount)
    {
        hp -= amount;
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
    }

    void Update()
    {

    }
}
