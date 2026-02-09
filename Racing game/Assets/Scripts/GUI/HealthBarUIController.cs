using UnityEngine;
using UnityEngine.UI;

public class HealthBarUIController : MonoBehaviour
{

    [SerializeField] Image imgFill;
    [Header("Events")]
    [SerializeField] FloatEventChannelSO normalizedHealth;


    void UpdateHealthBar(float normalizedHealth)
    {
        imgFill.fillAmount = normalizedHealth;
    }

    private void OnEnable()
    {

        normalizedHealth.OnEventRaised += UpdateHealthBar;
    }

    private void OnDisable()
    {
        normalizedHealth.OnEventRaised -= UpdateHealthBar;
    }

}
