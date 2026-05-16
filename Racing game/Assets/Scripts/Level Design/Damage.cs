using System;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] public int damage;
    private string topParentTag;
    public void Start()
    {
        topParentTag = MainGameSceneManager.instance.GetParentTag(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponentInParent<IDamageable>();

        if (damageable == null) return;
        if (MainGameSceneManager.instance.GetParentTag(other.gameObject).Equals(topParentTag))
            return;
        Debug.Log($"Hit on {other.name} for {damage}");
        damageable.TakeDamage(damage);       
    }
}
