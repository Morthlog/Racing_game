using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] VoidEventChannelSO shieldDisabled;

    private void OnDisable()
    {
        shieldDisabled.RaiseEvent();
    }
}
