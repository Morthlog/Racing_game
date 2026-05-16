using UnityEngine;
using UnityEngine.VFX;

public class VFXSoundController : MonoBehaviour
{
    public AudioSource fireworkSource;
    private VisualEffect vfx;

    void Start()
    {
        vfx = GetComponent<VisualEffect>();
        vfx.outputEventReceived += OnVFXOutputEvent;
        print("start");
    }

    private void OnVFXOutputEvent(VFXOutputEventArgs args)
    {
        if (args.nameId == Shader.PropertyToID("OnExplosion"))
        {
            print("event");
            fireworkSource.Play();
        }
    }
}