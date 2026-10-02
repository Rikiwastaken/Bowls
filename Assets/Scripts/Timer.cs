using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public static Timer instance;

    public float TimeSinceStarted;
    [SerializeField] private TextMeshProUGUI TimerText;
    public bool StopTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!StopTimer)
        {
            TimeSinceStarted += Time.deltaTime;
            TimerText.text = CreateTimeString();
        }


    }
    private string CreateTimeString()
    {
        int totalseconds = (int)TimeSinceStarted;
        int minutes = totalseconds / 60;
        int seconds = totalseconds % 60;
        string result = "";

        if (minutes <= 0)
        {
            result = "" + seconds;
        }
        else
        {
            string secondstring = "";
            if (seconds <= 9)
            {
                secondstring = "0" + seconds;
            }
            else
            {

                secondstring = "" + seconds;
            }
            result = minutes + ":" + secondstring;
        }

        return result;

    }
}
