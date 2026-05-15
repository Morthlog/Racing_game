using Unity.Cinemachine;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float speed = 50f;
    [SerializeField] float selfDestructionTime = 5f;
    float timeAfterLaunch = 0;
    private string owner;
    private Rigidbody ridigbody;
    private bool isLaunched = false;
    private Animator animator;

    private void Awake()
    {
        ridigbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    public void PrepareToLaunch()
    {
        animator.SetBool("Launch", true);
    }

    public void Launch()
    {
        ridigbody.isKinematic = false;
        transform.SetParent(null);
        ridigbody.AddForce(transform.forward* speed, ForceMode.VelocityChange);

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
