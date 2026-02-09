using UnityEngine;
using UnityEngine.Events;

public class IntEventListener : MonoBehaviour
{
    [SerializeField] private IntEventChannelSO channel;
    [SerializeField] private UnityEvent<int> response;

    private void OnEnable()
    {
        if (channel != null) channel.OnEventRaised += OnEventRaised;
    }

    private void OnDisable()
    {
        if (channel != null) channel.OnEventRaised -= OnEventRaised;
    }

    private void OnEventRaised(int value)
    {
        response?.Invoke(value);
    }
}
