using Unity.Cinemachine;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float speed = 50f;
    [SerializeField] GameObject effects;
    [SerializeField] float selfDestructionTime = 5f;
    float timeAfterLaunch=0;
    private string owner;    
    private Rigidbody ridigbody;
    private bool isLaunched = false;
    
    private void Awake()
    {
        ridigbody = GetComponent<Rigidbody>();
    }

    public void Launch()
    {
        //reseting first, because parent object movement causes bugs.
        ridigbody.linearVelocity = Vector3.zero;

        transform.SetParent(null);
        ridigbody.isKinematic = false;
        ridigbody.AddForce(transform.forward* speed, ForceMode.VelocityChange);

        effects.SetActive(true);
        isLaunched=true;
    }

    public void SetOwner(string name)
    {
        owner = name;
    }

    private void Update()
    {
        if (!isLaunched) return;
        timeAfterLaunch+= Time.deltaTime;

        if (timeAfterLaunch >= selfDestructionTime)
        {
            SelfDestruct();
        }  
    }

    private void OnCollisionEnter(Collision other)
    {
        print(other.gameObject.name);
        if (other.transform.root.CompareTag(owner) || !isLaunched) return;
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Destroy();     
        }
        
        SelfDestruct();
    }

    private void SelfDestruct()
    {
        //instantiating the effect so we can destroy this gameobject without setting timers,to wait for the effect to finish
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
