using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarConsoleUI : MonoBehaviour
{
    private TextMeshProUGUI speedometer;
    private int currentSpeed = 0;
    private Rigidbody carRb;

    [SerializeField] private Image speedometerFillImage;
    [SerializeField] private Gradient speedGradient;
    [SerializeField] private float maxSpeed = 100f;
    private const float fillAmmount = 0.5f;
    [SerializeField][Range(0f, fillAmmount)] private float currentFillAmount = fillAmmount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        speedometer = GameObject.FindGameObjectWithTag("CarConsoleUI").GetComponent<TextMeshProUGUI>();
        carRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 flatVelocity = carRb.linearVelocity;
        flatVelocity.y = 0f;
        SetSpeed(flatVelocity.magnitude);
        speedometer.text = $"{currentSpeed}\nkm/h";

        SetSpeedometerFill(currentSpeed);
    }

    void SetSpeedometerFill(float speed)
    {
        float speedPercentage = Mathf.Clamp01(speed / maxSpeed);
        float finalFill = speedPercentage * fillAmmount;//multiplying by fill to get final fill
        speedometerFillImage.fillAmount = finalFill;
        speedometerFillImage.color = speedGradient.Evaluate(speedPercentage);
    }

    public void SetSpeed(float val)
    {
        currentSpeed = (int) (val * 3.6);
    }

}
