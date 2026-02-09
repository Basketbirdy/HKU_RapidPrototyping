using UnityEngine;

public interface IDamagable
{
    public float CurrentHealth { get; }
    public bool IsInvincible { get; }
    public void TakeDamage(float damage);
    public void Die();
}
