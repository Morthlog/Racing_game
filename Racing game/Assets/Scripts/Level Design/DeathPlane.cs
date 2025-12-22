using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameLoopManager.instance.getParentTag(other.gameObject) == "Player")
        {
            GameLoopManager.instance.toLastCheckpoint();
        }
    }
}
