using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] VoidEventChannelSO missileUsed;
    [SerializeField] PowerUpTypeChannelSO powerUpPicked;
    [SerializeField] GameObject missilePrefab;
    [SerializeField] GameObject missileBase;
    [SerializeField] Transform missileSpawnPoint;
    [SerializeField] float speed;

    GameObject missile;
    Missile missileScript;
    bool missileLaunched = false;

    private void Start()
    {
        missileBase.SetActive(true);
    }


    void CreateMissile(PowerupType type)
    {
        if (type != PowerupType.Missile) return;

        missile = Instantiate(missilePrefab, missileSpawnPoint);
        missileScript=missile.GetComponent<Missile>();
        missileScript.SetOwner(tag);
        missileLaunched = false;
    }
    void LaunchMissile()
    {
        if (missileLaunched) return;
        missileScript.PrepareToLaunch();
        missileLaunched = true;
    }

    private void OnEnable()
    {
        powerUpPicked.OnEventRaised += CreateMissile;
        missileUsed.OnEventRaised += LaunchMissile;
    }

    private void OnDisable()
    {
        powerUpPicked.OnEventRaised -= CreateMissile;
        missileUsed.OnEventRaised -= LaunchMissile;
    }
}
