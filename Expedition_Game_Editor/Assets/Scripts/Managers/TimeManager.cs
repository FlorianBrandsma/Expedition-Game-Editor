using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class TimeManager : MonoBehaviour
{
    public class TimeFrame
    {
        public int StartTime { get; set; }
        public int EndTime { get; set; }

        public bool CheckTimeConflict(TimeFrame changedTime)
        {
            //Check when the changed start time is before the changed end time
            if (changedTime.StartTime <= changedTime.EndTime)
            {
                //Check whether the changed time frame conflicts with the existing
                if (changedTime.EndTime >= StartTime && changedTime.StartTime <= StartTime)
                    return true;

                if (changedTime.EndTime >= EndTime && changedTime.StartTime <= EndTime)
                    return true;

            } else {

                //Check when the changed start time is after the changed end time (passing the 24 hour mark)
                if (StartTime <= EndTime)
                {
                    //If the existing start time is before the existing end time, simply check whether
                    //the existing time is inbetween the changed time
                    if (changedTime.EndTime >= StartTime)
                        return true;

                    if (changedTime.StartTime <= EndTime)
                        return true;

                } else {

                    //It is not possible for two time frames to pass the 24 hour mark, so if the existing
                    //start time is also before the existing end time, there will always be a time conflict

                    return true;
                }
            }

            return false;
        }
    }

    static public TimeManager instance;

    static public bool active;

    static public Light activeLight;

    private float lightValue;

    public Color baseColor;
    public Gradient nightDayColor;
    public Gradient ambientColor;

    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    static public int hoursInDay = 24;
    static public int secondsInHour = 3600;

    //24 = 1 game time cycle per real hour
    //Too high speed messes with the navigation mesh updates
    static public float gameTimeSpeed;

    static public int defaultTime = (12 * 60 * 60);

    private float activeTimeScale = 1;

    private int activeHour;
    private int activeTime;
    
    public float TimeScale
    {
        get { return Time.timeScale; }
        set
        {
            Time.timeScale = value;

            if(value > 0)
                activeTimeScale = value;

            if (GameManager.instance != null)
            {
                GameManager.instance.gamePauseAction.UpdateAction();
                GameManager.instance.gameSpeedAction.UpdateAction();
            }
        }
    }

    public bool Paused
    {
        get { return Time.timeScale == 0; }
    }

    public int ActiveTime
    {
        get { return activeTime; }
        set
        {
            activeTime = value;
            
            SetLighting(activeTime);
        }
    }

    private int ActiveHour
    {
        get { return activeHour; }
        set
        {
            if (activeHour == value) return;
            
            GameManager.instance.CheckTime();

            activeHour = value;
        }
    }
    
    public void Awake()
    {
        instance = this;

        TimeScale = 1;

        InitializeTime();
    }

    private void Update()
    {
        if (active)
        {
            float counter = 1 * Time.deltaTime * gameTimeSpeed;
            
            if (ActiveTime + Mathf.FloorToInt(counter) < (hoursInDay * secondsInHour) - 1)
                ActiveTime += Mathf.FloorToInt(counter);
            else
                ActiveTime = 0;

            GameManager.instance.gameSaveData.playerSaveData.GameTime = ActiveTime;

            //Might be better suited in ActiveTime property
            if (GameManager.instance.gameTimeAction.Dropdown != null)
            {
                GameManager.instance.gameTimeAction.UpdateAction();

            } else {

                ActiveHour = ActiveTime / secondsInHour;
            }
        }
    }
    
    public void InitializeTime()
    {
        activeTime = defaultTime;
    }

    public void InitializeGameTime(int time)
    {
        ActiveTime = time;
    }

    public void PauseTime(bool pause)
    {
        TimeScale = pause ? 0 : activeTimeScale;
    }
    
    public void SetEditorTime(int time, bool resetEditor = false)
    {
        activeTime = time;

        SetLighting(activeTime);

        //No need to reload data for interactions - it's there and filtered by the organizer
        if (resetEditor)
            RenderManager.ResetPath(false);
    }

    public void SetGameTime(int time)
    {
        activeTime = time;

        SetLighting(time);

        GameManager.instance.CheckTime();
    }

    public void SetLighting()
    {
        SetLighting(activeTime);
    }

    public void ResetLighting()
    {
        SetLighting(defaultTime);

        activeLight.color = baseColor;
    }

    public void SetLighting(int time)
    {
        lightValue = Mathf.Clamp01((float)time / secondsInHour / hoursInDay);

        RenderSettings.ambientLight = ambientColor.Evaluate(lightValue);

        RenderSettings.fogColor = nightDayFogColor.Evaluate(lightValue);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(lightValue) * fogScale;

        SetCameraLight(activeLight);
    }

    public void SetCameraLight(Light light)
    {
        if (light == null) return;

        light.color = nightDayColor.Evaluate(lightValue);
    }

    static public string TimeFromSeconds(int seconds)
    {
        var time = TimeSpan.FromSeconds(seconds);

        var hourFormat = "00";
        var totalHours = Mathf.FloorToInt(Convert.ToSingle(time.TotalHours));

        while (hourFormat.Length < totalHours.ToString().Length)
            hourFormat += "0";

        var timeString = string.Format("{0:" + hourFormat + "}:{1:00}:{2:00}", time.TotalHours, time.Minutes, time.Seconds);

        return timeString;
    }

    static public string FormatTime(int seconds)
    {
        var time = TimeSpan.FromSeconds(seconds);

        return string.Format("{0:00}:{1:00}", time.Hours, time.Minutes);
    }

    static public bool TimeFramesAvailable(IDataController dataController)
    {
        int usedFrames = 0;

        var timeFrameList = new List<TimeFrame>();

        switch(dataController.DataType)
        {
            case Enums.DataType.Atmosphere:

                timeFrameList = dataController.DataList.Cast<AtmosphereElementData>().Where(x => !x.Default).Select(x => new TimeFrame() { StartTime = x.StartTime, EndTime = x.EndTime }).ToList();

                break;

            case Enums.DataType.Interaction:
                
                timeFrameList = dataController.DataList.Cast<InteractionElementData>().Where(x => !x.Default).Select(x => new TimeFrame() { StartTime = x.StartTime, EndTime = x.EndTime }).ToList();
        
                break;  
        }

        timeFrameList.ForEach(x =>
        {
            if (x.StartTime <= x.EndTime)
                usedFrames += (x.EndTime + 1) - x.StartTime;
            else
                usedFrames += (hoursInDay - x.StartTime) + (x.EndTime + 1);
        });

        return (usedFrames < hoursInDay);
    }

    public int DefaultTime(List<int> defaultTimes)
    {
        var defaultTime = defaultTimes.Contains(ActiveTime) ? ActiveTime : defaultTimes.First();

        return defaultTime;
    }

    static public List<int> AvailableTimes(List<TimeFrame> timeFrameList)
    {
        var availableTimes = new List<int>();

        for (int i = 0; i < hoursInDay; i++)
        {
            if (!timeFrameList.Any(x => TimeInFrame(i * secondsInHour, x.StartTime, x.EndTime)))
                availableTimes.Add(i * secondsInHour);
        }

        return availableTimes;
    }

    static public bool TimeInFrame(int time, int startTime, int endTime)
    {
        if (startTime <= endTime)
        {
            if (time >= startTime && time <= endTime)
                return true;

        } else {

            if (time <= startTime && time >= 0 || time >= endTime && time <= hoursInDay)
                return true;
        }

        return false;
    }

    static public bool TimeConflict(IDataController dataController, IElementData changedData)
    {
        var dataList = new List<TimeFrame>();
        var changedTimeFrame = new TimeFrame();
        
        switch(dataController.DataType)
        {
            case Enums.DataType.Atmosphere:

                dataList = dataController.DataList.Cast<AtmosphereElementData>()
                                                  .Where(x => x != (AtmosphereElementData)changedData && !x.Default)
                                                  .Select(x => new TimeFrame()
                                                  {
                                                      StartTime = x.StartTime,
                                                      EndTime = x.EndTime

                                                  }).ToList();

                var atmosphereData = (AtmosphereElementData)changedData;

                changedTimeFrame = new TimeFrame() { StartTime = atmosphereData.StartTime, EndTime = atmosphereData.EndTime };

                break;

            case Enums.DataType.Interaction:

                dataList = dataController.DataList.Cast<InteractionElementData>()
                                                  .Where(x => x != (InteractionElementData)changedData && !x.Default)
                                                  .Select(x => new TimeFrame()
                                                  {
                                                      StartTime = x.StartTime,
                                                      EndTime = x.EndTime
                                                  }).ToList();

                var interactionData = (InteractionElementData)changedData;

                changedTimeFrame = new TimeFrame() { StartTime = interactionData.StartTime, EndTime = interactionData.EndTime };

                break;  
        }

        var conflict =  dataList.Any(x => x.CheckTimeConflict(changedTimeFrame)) ||
                        dataList.Any(x => changedTimeFrame.CheckTimeConflict(x));

        return conflict;
    }
}
