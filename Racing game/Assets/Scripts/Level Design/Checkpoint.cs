using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    [Header("Tags to check")]
    [SerializeField] string[] allowedTags = {"Player"};
    [SerializeField] private string contactOn = "Body";

    private int id;

    public int ID { get { return id; } }
    void Start()
    {
        id = int.Parse(this.name[^1].ToString());
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
