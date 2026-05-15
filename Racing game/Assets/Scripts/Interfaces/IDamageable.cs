public interface IDamageable
{
    // Lower the hp of the component by dmg if not in i-frame
    void TakeDamage(int dmg);

    void Destroy();
}
