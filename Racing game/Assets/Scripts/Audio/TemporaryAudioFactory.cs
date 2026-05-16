using UnityEngine;


public class TemporaryAudioFactory: MonoBehaviour
{
    [SerializeField] AudioClip menuClip;
    public void PlayTemporaryMenuSound()
    {
        GameObject temporaryAudioGameobject = new GameObject("TemporaryMenuAudio");

        AudioSource audioSource = temporaryAudioGameobject.AddComponent<AudioSource>();
        audioSource.clip = menuClip;

        audioSource.Play();

        // we dont allow the gameobject to be destroyed until the audio is completed
        DontDestroyOnLoad(temporaryAudioGameobject);
        Destroy(temporaryAudioGameobject, menuClip.length);
    }
}
