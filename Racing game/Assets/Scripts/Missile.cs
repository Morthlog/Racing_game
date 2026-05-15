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

    [SerializeField] private float destructionRadius = 5.0f;
    [SerializeField] private Color radiusGizmoColor = Color.green;
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
         
        SelfDestruct();
    }

    private void SelfDestruct()
    {
        //instantiating the effect so we can destroy this gameobject without setting timers,to wait for the effect to finish
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        DestroyEverythingInRadius();
        Destroy(gameObject);
    }

    void DestroyEverythingInRadius()
    {
        Vector3 center = transform.position;
        Collider[] hitColliders = Physics.OverlapSphere(center, destructionRadius);

        foreach (Collider collider in hitColliders)
        {
            if (collider.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Destroy();
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = radiusGizmoColor;
        Gizmos.DrawWireSphere(transform.position, destructionRadius);
    }
}
