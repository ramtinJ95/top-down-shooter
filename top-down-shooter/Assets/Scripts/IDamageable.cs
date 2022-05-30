using UnityEngine;

public interface IDamageable
{
    public void TakeHit(float damage, RaycastHit hit);
    public void TakeDamage(float damage);
}