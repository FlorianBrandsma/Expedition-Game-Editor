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

    private float dot;

    static public Color test;

    public Color baseColor;
    public Gradient nightDayColor;
    public Gradient ambientColor;

    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    static public int hoursInDay = 24;

    static public int defaultTime = 12;
    static public int activeTime;

    public void Awake()
    {
        instance = this;

        InitializeTime();
    }

    public void InitializeTime()
    {
        activeTime = defaultTime;
    }

    public void SetTime(int time, bool resetEditor = false)
    {
        activeTime = time;

        SetLighting(activeTime);

        //No need to reload data for time - it's there and filtered by the organizer
        if (resetEditor)
            RenderManager.ResetPath(false);
    }

    public void SetLighting()
    {
        SetLighting(activeTime);
    }

    public void ResetLighting(Light light)
    {
        SetLighting(defaultTime);

        light.color = baseColor;
    }

    public void SetLighting(int time)
    {
        dot = Mathf.Clamp01((float)time / hoursInDay);

        RenderSettings.ambientLight = ambientColor.Evaluate(dot);

        RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;
    }

    public void SetCameraLight(Light light)
    {
        light.color = nightDayColor.Evaluate(dot);
    }

    static public string FormatTime(int time, bool isStart = false)
    {
        return time.ToString("D2") + (isStart ? ":00" : ":59");
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

    static public int DefaultTime(List<int> defaultTimes)
    {
        var defaultTime = defaultTimes.Contains(activeTime) ? activeTime : defaultTimes.First();

        return defaultTime;
    }

    static public List<int> AvailableTimes(List<TimeFrame> timeFrameList)
    {
        var availableTimes = new List<int>();

        for (int i = 0; i < hoursInDay; i++)
        {
            if (!timeFrameList.Any(x => TimeInFrame(i, x.StartTime, x.EndTime)))
                availableTimes.Add(i);
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
