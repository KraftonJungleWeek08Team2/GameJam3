using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] float moveSpeed;

    public int hp { get; private set; }

    public void TakeDamage(int amount)
    {
        hp -= amount;
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
        if (transform.position.x > 3)
        {
            transform.position += Vector3.left * Time.deltaTime * moveSpeed;
        }
    }
}
