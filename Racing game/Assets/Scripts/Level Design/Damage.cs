using System;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] public int damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            Debug.Log($"Hit on {other.name} for {damage}");
            damageable.TakeDamage(damage);
        }
            
    }
}
