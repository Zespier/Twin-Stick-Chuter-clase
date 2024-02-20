using UnityEngine;
public interface IDamageable<T>
{
    bool IsDead();
    void TakeDamage(T damage, Vector3 impactPosition = default);
}