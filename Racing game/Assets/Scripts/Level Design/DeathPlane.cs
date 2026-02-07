using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameLoopManager.instance.GetParentTag(other.gameObject) == "Player")
        {
            var damageable = other.GetComponent<IDamageable>() ?? other.GetComponentInParent<IDamageable>();

            if (damageable == null) return;

            damageable.TakeDamage(int.MaxValue);
        }
    }
}
