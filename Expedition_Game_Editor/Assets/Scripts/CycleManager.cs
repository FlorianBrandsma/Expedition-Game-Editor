using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class CycleManager : MonoBehaviour
{
    public Gradient nightDayColor;
    public Gradient ambientColor;

    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;
   
    float curTime = 0;
    float totalCycles = 10000; //Amount of Day/Night cycles within 24 hours

    float day, hour, second, minute;

    Light mainLight;

    public bool stopTime;

    public Text timeText;

    void Start()
    {
        mainLight = GetComponent<Light>();

        StartTime();
    }

    void StartTime()
    {
        //Bereken de tijd gedeeld door cycles
        hour   = Mathf.Floor(System.DateTime.Now.Hour   * totalCycles);
        minute = Mathf.Floor(System.DateTime.Now.Minute * totalCycles) + (((System.DateTime.Now.Hour   * totalCycles) % 1) * 60f);
        second = Mathf.Floor(System.DateTime.Now.Second * totalCycles) + (((System.DateTime.Now.Minute * totalCycles) % 1) * 60f);

        SetTime(); 
    }

    void SetTime()
    {
        second += 1 * (Time.deltaTime * totalCycles);

        while (second >= 60)
        {
            second -= 60;
            minute++;
        }

        while (minute >= 60)
        {
            minute -= 60;
            hour++;
        }

        while (hour >= 24)
        {
            hour -= 24;
            day++;
        }

        //Debug.Log(day + ":" + hour + ":" + minute + ":" + second);
    }

	void Update ()
    {
        //GAMETIME
        SetTime();

        curTime = (((((hour * 60) + minute) * 60) + second) / 86400f);
        timeText.text = "Day " + day + ", " + hour + ":" + minute + ":" + second;

        //REALTIME
        //timeText.text = "Clamp01 " + (((((System.DateTime.Now.Hour * 60) + System.DateTime.Now.Minute) * 60) + System.DateTime.Now.Second) / 86400f);
        //curTime = (((((System.DateTime.Now.Hour * 60) + System.DateTime.Now.Minute) * 60) + System.DateTime.Now.Second) / 86400f);

        float tRange = 1;
        float dot = Mathf.Clamp01((curTime) / tRange);
        
        mainLight.color = nightDayColor.Evaluate(dot);
        RenderSettings.ambientLight = ambientColor.Evaluate(dot);

        RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;
    }
}
