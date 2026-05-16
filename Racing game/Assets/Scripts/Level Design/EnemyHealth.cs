using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int currentHealth = 1;

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            transform.gameObject.SetActive(false);
        }
    }

    public void Destroy()
    {
        TakeDamage(currentHealth);
    }
}
