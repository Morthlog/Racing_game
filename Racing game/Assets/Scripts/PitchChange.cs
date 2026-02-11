using UnityEngine;

public class PitchChange : MonoBehaviour
{
    private AudioSource m_AudioSource;
    [Header("Pitch Ranges")]
    [SerializeField] private float min = 0.9f;
    [SerializeField] private float max = 1.1f;

    public void ChangePitch(AudioSource audioSource)
    {
        audioSource.pitch = Random.Range(min, max);
    }
}
