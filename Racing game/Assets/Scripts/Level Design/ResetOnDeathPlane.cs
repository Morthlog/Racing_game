using UnityEngine;

public class ResetOnDeathPlane : MonoBehaviour
{
    private GameObject og;
    void Start()
    {
        og = new GameObject();
        copyTransform(og, gameObject);
    }

    private void OnReset()
    {
        copyTransform(gameObject, og);
        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        gameObject.SetActive(false);
    }

    void copyTransform(GameObject og, GameObject other)
    {
        og.transform.position = other.transform.position;
        og.transform.rotation = other.transform.rotation;
        og.transform.localScale = other.transform.localScale;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DeathPlane"))
            OnReset();

    }

}
