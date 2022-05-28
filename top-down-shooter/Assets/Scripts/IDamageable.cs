using UnityEngine;

public interface IDamageable
{
    public void TakeHit(float damage, RaycastHit hit);
}