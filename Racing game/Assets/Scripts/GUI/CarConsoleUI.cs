using TMPro;
using UnityEngine;

public class CarConsoleUI : MonoBehaviour
{
    private TextMeshProUGUI speedometer;
    private int currentSpeed = 0;
    private Rigidbody carRb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedometer = GetComponentInChildren<TextMeshProUGUI>();
        carRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 flatVelocity = carRb.linearVelocity;
        flatVelocity.y = 0f;
        setSpeed(flatVelocity.magnitude);
        speedometer.text = $"{currentSpeed} km/h";
    }

    public void setSpeed(float val)
    {
        currentSpeed = (int) (val * 3.6);
    }

}
