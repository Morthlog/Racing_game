using UnityEngine;
using UnityEngine.Events;

public class VoidEventListener : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO channel;
    [SerializeField] private UnityEvent response;

    private void OnEnable()
    {
        if (channel != null) channel.OnEventRaised += OnEventRaised;
    }

    private void OnDisable()
    {
        if (channel != null) channel.OnEventRaised -= OnEventRaised;
    }

    private void OnEventRaised()
    {
        response?.Invoke();
    }
}
