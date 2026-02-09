using System.Collections;
using UnityEngine;

public class CarSetup : MonoBehaviour
{
    [SerializeField] GameObject wheelColliderPrefab;
    [SerializeField] PlayerData playerData;
    [SerializeField] GameObject lights, shield;
    GameObject wheelsGO;
    GameObject car;

    [Header("Events")]
    [SerializeField] VoidEventChannelSO playerDied;
    [SerializeField] IntEventChannelSO shieldUsed;

    void Awake()
    {
        car = Instantiate(playerData.chosenCar, transform);

        wheelsGO = GameObject.Find("wheels");

        CreateWheelColliderGO("wheel front right", true, true);
        CreateWheelColliderGO("wheel front left", true, true);
        CreateWheelColliderGO("wheel back right");
        CreateWheelColliderGO("wheel back left");


        MeshCollider[] meshColliders = GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider collider in meshColliders)
        {
            if (collider.name.ToLower().Contains("wheel"))
            {
                collider.enabled = false;
            }
            else
            {
                collider.convex = true;
            }

            if (collider.name.Equals("body"))
            {
                collider.gameObject.tag = "Body";
            }
        }
    }

    GameObject CreateWheelColliderGO(string name, bool steerable = false, bool motorized = false)
    {
        Transform currentWheel = wheelsGO.transform.Find(name);
        GameObject wheelColliderGO = Instantiate(wheelColliderPrefab, currentWheel.position, Quaternion.identity, wheelsGO.transform);
        wheelColliderGO.name = name + " collider";

        WheelControl wheelControl = wheelColliderGO.GetComponent<WheelControl>();
        wheelControl.steerable = steerable;
        wheelControl.motorized = motorized;
        wheelControl.wheelModel = currentWheel.transform;

        return wheelColliderGO;
    }

    private void DisableCar()
    {
        car.SetActive(false);
    }

    private void EnableCar()
    {
        car.SetActive(true);
    }

    private void EnableLights()
    {
        lights.SetActive(true);
    }

    private void DisableLights()
    {
        lights.SetActive(false);
    }

    public void OnDeath()
    {
        DisableCar();
        DisableLights();
    }

    public void OnRestart()
    {
        EnableCar();
        EnableLights();
    }
    void ActivateShield(int duration)
    {
        shield.SetActive(true);
        StartCoroutine(DeactivateShield(duration));
    }


    IEnumerator DeactivateShield(int duration)
    {
        yield return new WaitForSeconds(duration);
        shield.SetActive(false );
    }
    private void OnEnable()
    {
        playerDied.OnEventRaised += OnDeath;
        shieldUsed.OnEventRaised += ActivateShield;
    }

    private void OnDisable()
    {
        playerDied.OnEventRaised -= OnDeath;
        shieldUsed.OnEventRaised -= ActivateShield;
    }
}
