using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private float cycle = 0.1f;

    [Header("AudioSources")]
    [SerializeField] AudioSource backgroundMusic;
    [SerializeField] AudioSource countdownCounting;
    [SerializeField] AudioSource countdownDone;
    PitchChange pitchChanger;
    private float timer = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pitchChanger = GetComponent<PitchChange>();
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusic.Play();
    }

    public void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    public void SetAudioSnapshot(AudioMixerSnapshot snapshot)
    {
        snapshot.TransitionTo(2f);
    }
    // Update is called once per frame
    void Update()
    {
        if (timer + cycle > Time.time)
            return;
        timer = Time.time;
        pitchChanger.ChangePitch(backgroundMusic);
    }

    public void PlayCountdownCounting()
    {
        countdownCounting.Play();
    }

    public void PlayCountdownDone()
    {
        countdownDone.Play();
    }
}
