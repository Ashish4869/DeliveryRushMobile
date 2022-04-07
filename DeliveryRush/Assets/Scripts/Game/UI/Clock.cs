using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Clock : MonoBehaviour
{
    /// <summary>
    /// Maintains the clock time
    /// </summary>

    [SerializeField]
    TextMeshProUGUI ClockTime;

    [SerializeField]
    float StartHour;

    [SerializeField]
    string AMorPM;
    private const float REAL_TIME_TO_MIN_IN_GAME = 6f;
    float currentTime=0;
    float hourTime = 0f;
    float MinTime = 0;
    string AM_PM = "AM";

    // Start is called before the first frame update
    void Start()
    {
        hourTime = StartHour;
        AM_PM = AMorPM;
        ClockTime.text = hourTime.ToString("00") + ":" + MinTime.ToString("00") + " "+ AM_PM;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime / REAL_TIME_TO_MIN_IN_GAME;
       
       if(currentTime > 1)
       {
            UpdateMin();
       }
    }

    void UpdateMin()
    {
        currentTime = 0;
        MinTime += 1;

        if(MinTime > 59)
        {
            UpdateHour();
        }

        UpdateTime();
    }

    void UpdateHour()
    {
        hourTime = ( hourTime + 1 ) % 13;

        if(hourTime == 0)
        {
            hourTime = 1;
        }

        MinTime = 0;

        if(hourTime == 12)
        {
            ChangeAMtoPM();
        }

        UpdateTime();
    }

    void ChangeAMtoPM()
    {
        AM_PM = "PM";
        hourTime = 12;
    }

    void UpdateTime() => ClockTime.text = hourTime.ToString("00") + ":" + MinTime.ToString("00")+ " " + AM_PM;

    public string GetTime() => hourTime.ToString("00") + ":" + MinTime.ToString("00") + " " + AM_PM;

}
