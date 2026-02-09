using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (GameLoopManager.instance.GetParentTag(other.gameObject) == "Player")
        {
            var damageable = other.GetComponentInParent<IDamageable>();

            if (damageable == null) return;

            damageable.Die();
        }
    }
}
