using UnityEngine;

public class CarPrefabSetup:MonoBehaviour
{
    [SerializeField]
    GameObject wheelColliderPrefab;
    GameObject wheelsGO;

    [SerializeField]
    PlayerData playerData;
    void Awake()
    {
        Instantiate(playerData.chosenCar, transform);
      
        wheelsGO = GameObject.Find("wheels");

        CreateWheelColliderGO("wheel front right", true, true);
        CreateWheelColliderGO("wheel front left", true, true);
        CreateWheelColliderGO("wheel back right");
        CreateWheelColliderGO("wheel back left");   


        MeshCollider[] meshColliders = GetComponentsInChildren<MeshCollider>();
        foreach (MeshCollider collider in meshColliders)
        {    
            if(collider.name.ToLower().Contains("wheel"))
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

    GameObject CreateWheelColliderGO(string name, bool steerable = false, bool motorized=false)
    {
        Transform currentWheel = wheelsGO.transform.Find(name);
        GameObject wheelColliderGO = Instantiate(wheelColliderPrefab, currentWheel.position, Quaternion.identity, wheelsGO.transform);
        wheelColliderGO.name = name + " collider";

        WheelControl wheelControl= wheelColliderGO.GetComponent<WheelControl>();
        wheelControl.steerable = steerable;
        wheelControl.motorized = motorized;
        wheelControl.wheelModel = currentWheel.transform;
        
        return wheelColliderGO;
    }


}
