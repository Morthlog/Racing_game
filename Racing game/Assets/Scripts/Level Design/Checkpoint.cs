using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    
    [SerializeField] private int id;
    [SerializeField] private int nextCheckpointID = 0;
    [Header("Tags to check")]
    [SerializeField] string[] allowedTags = {"Player"};
    [SerializeField] private string contactOn = "Body";

    public int ID { get { return id; } }

    public void SetID (int id)
    {
        this.id = id;
    }    

    public void SetNextID(int nextCheckpointID)
    {
        this.nextCheckpointID = nextCheckpointID;
    }
    public int NextID()
    {
        return nextCheckpointID;
    }
    private void OnTriggerEnter(Collider other)
    {
        foreach (string tag in allowedTags)
        {
            if (other.CompareTag(contactOn) && GameLoopManager.instance.getParentTag(other.gameObject) == tag)
            {
                GameLoopManager.instance.OnCheckpointHit(other.gameObject, ID);
            }
        }
        
    }

}
