using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (MainGameSceneManager.instance.GetParentTag(other.gameObject) == "Player")
        {
            var damageable = other.GetComponentInParent<IDamageable>();

            if (damageable == null) return;

            damageable.Die();
        }
    }
}
