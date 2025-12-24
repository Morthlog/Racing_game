using NUnit.Framework.Interfaces;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    private float closedRotation;
    private float openedRotation;
    private GameObject handle;
    private RotateOnAxis rotateHandle;
    private GameObject[] ammo;
    private int currentShot = 0;
    private State state;
    private float timer;
    [SerializeField] float reloadingCooldown = 10f;
    [SerializeField] float shootingCooldown = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject ammoTemplate = null;
        ammo = new GameObject[6];
        foreach (Transform transform in gameObject.transform)
        {
            if (transform.CompareTag("Spikes"))
                ammoTemplate = transform.gameObject;
            else if (transform.CompareTag("Handle"))
                handle = transform.gameObject;
        }
        ammoTemplate.gameObject.SetActive(false);
        for (int i = 0; i < ammo.Length; i++)
        {
            ammo[i] = Instantiate(ammoTemplate);
            ammo[i].transform.SetParent(transform, false);
        }
        
        rotateHandle = handle.GetComponent<RotateOnAxis>();
        closedRotation = rotateHandle.GetAngle();
        openedRotation = closedRotation - 90f;

        state = State.OpenCover;
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time <= timer)
            return;
        switch (state)
        {
            case State.OpenCover:
                rotateHandle.SetCanRotate(true);
                rotateHandle.SetDirection(RotateOnAxis.Direction.Negative);
                state = State.OpeningCover;
                break;
            case State.OpeningCover:
                if (rotateHandle.GetAngle() > openedRotation)
                    break;
                rotateHandle.SetCanRotate(false);
                state = State.Shooting;
                break;
            case State.Shooting:
                ammo[currentShot].SetActive(true);
                timer = Time.time + shootingCooldown;

                state = State.CloseCover;
                break;

            case State.CloseCover:
                rotateHandle.SetCanRotate(true);
                rotateHandle.SetDirection(RotateOnAxis.Direction.Positive);
                state = State.ClosingCover;
                break;
            case State.ClosingCover:
                if (rotateHandle.GetAngle() < closedRotation)
                    break;

                rotateHandle.SetCanRotate(false);
                state = State.Reloading;
                break;
            case State.Reloading:
                currentShot = (currentShot + 1) % ammo.Length;

                timer = Time.time + reloadingCooldown;
                state = State.OpenCover;
                break;

        }
    }

    enum State
    {
        OpeningCover,
        OpenCover,
        Shooting,
        ClosingCover,
        CloseCover,
        Reloading
    }
}
