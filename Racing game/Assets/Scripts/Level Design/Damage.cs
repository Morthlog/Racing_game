using System;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] public int damage;

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>() ??other.GetComponentInParent<IDamageable>();

        if (damageable == null) return;
        
        Debug.Log($"Hit on {other.name} for {damage}");
        damageable.TakeDamage(damage);       
    }
}
