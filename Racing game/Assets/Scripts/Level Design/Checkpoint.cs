using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    
    [SerializeField] private int id;
    [SerializeField] private int[] nextCheckpointID = new int[3];
    [Header("Tags to check")]
    [SerializeField] string[] allowedTags = {"Player"};
    [SerializeField] private string contactOn = "Body";
    [SerializeField] private bool isStart = false;
    public int ID { get { return id; } }

    public bool IsStart()
    {
        return isStart;
    }
    public void SetID (int id)
    {
        this.id = id;
    }    

    public void SetNextIDs(int[] nextCheckpointID)
    {
        this.nextCheckpointID = nextCheckpointID;
    }
    public int[] NextIDs()
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
