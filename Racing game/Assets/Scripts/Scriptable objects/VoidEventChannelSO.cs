using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Void Event Channel",
fileName = "VoidEventChannel")]
public class VoidEventChannelSO : ScriptableObject
{
    [Tooltip("The action to perform")]
    public UnityAction OnEventRaised;


    //with the context menu can manually raise the event from the options menu of the object  
    [ContextMenu("Raise Event")]
    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }
}