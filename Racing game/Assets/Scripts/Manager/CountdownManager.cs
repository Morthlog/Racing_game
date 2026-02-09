using System.Collections;
using UnityEngine;

public class CountdownManager : MonoBehaviour
{
    GameObject countDown;

    public static CountdownManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        countDown = GameObject.FindGameObjectWithTag("Countdown");
        StartCountdown();
    }

    public void StartCountdown()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        yield return new WaitForSeconds(0.5f); // Start the countdown after a small wait
        countDown.GetComponent<Countdown>().StartCountdown();
    }

    public void AllowMovement()
    {
        Debug.Log("GO GO GO");
        GameLoopManager.instance.StartRound();
    }
}
