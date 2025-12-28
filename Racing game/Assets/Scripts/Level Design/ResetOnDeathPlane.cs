using UnityEngine;

public class ResetOnDeathPlane : MonoBehaviour
{
    private TransformData defTrans;
    void Start()
    {
        defTrans = new TransformData();
        defTrans.localPosition = transform.localPosition;
        defTrans.rotation = transform.rotation;
        defTrans.localScale = transform.localScale;
    }

    private void OnReset()
    {
        ResetTransform();
        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            ResetRigidbody(rb);
        }
        gameObject.SetActive(false);
    }

    void ResetTransform()
    {
         transform.SetLocalPositionAndRotation(defTrans.localPosition, defTrans.rotation);
         transform.localScale = defTrans.localScale;
    }

    void ResetRigidbody(Rigidbody rb)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DeathPlane"))
            OnReset();
    }

    private struct TransformData
    {
        public Vector3 localPosition;
        public Quaternion rotation;
        public Vector3 localScale;
    }

}
