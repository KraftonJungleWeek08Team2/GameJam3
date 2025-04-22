using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    public int hp{ get; private set; }
    
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
        Debug.Log("Player has died.");
    }
}
