using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    void IDamageable.Destroy()
    {
        this.gameObject.SetActive(false);
    }


    void IDamageable.TakeDamage(int dmg)
    {
        throw new System.NotImplementedException();
    }
}
