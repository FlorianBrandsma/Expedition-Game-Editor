using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

public class TimeManager : MonoBehaviour
{
    public class TimeFrame
    {
        public int StartTime    { get; set; }
        public int EndTime      { get; set; }

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

    public class TimeEvent
    {
        public int Time { get; set; }
        public List<GameWorldInteractableElementData> WorldInteractableDataList { get; set; } = new List<GameWorldInteractableElementData>();
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

    private TimeFrame activeTime = new TimeFrame();

    static public List<TimeEvent> timeEventList;
    static public List<GameInteractionElementData> activeInteractionList;

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
        get { return activeTime.EndTime; }
        set
        {
            activeTime.StartTime = activeTime.EndTime;
            activeTime.EndTime = value;

            SetLighting(activeTime.EndTime);

            if (active)
                SetGameTime(activeTime.EndTime);
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
        if (active && !Paused)
        {
            float counter = 1 * Time.deltaTime;

            float gameTimeCounter = counter * gameTimeSpeed;

            if (ActiveTime + Mathf.FloorToInt(gameTimeCounter) < (hoursInDay * secondsInHour) - 1)
            {
                ActiveTime += Mathf.FloorToInt(gameTimeCounter);

            } else {
                
                ActiveTime -= ((hoursInDay * secondsInHour) - Mathf.FloorToInt(gameTimeCounter));
            }

            GameManager.instance.gameSaveData.PlayerSaveData.GameTime = ActiveTime;

            //Might be better suited in ActiveTime property
            if (GameManager.instance.gameTimeAction.Dropdown != null)
            {
                GameManager.instance.gameTimeAction.UpdateAction();
            }

            MovementManager.movableWorldInteractableList.ForEach(x =>
            {
                CountTravelTime(x, counter);
                CountPatience(x, counter);
            });
            
            CountInteractionDelay(counter);
            CountSceneDuration(counter);
        }
    }
    
    private void CountTravelTime(GameWorldInteractableElementData worldInteractableElementData, float counter)
    {
        if (worldInteractableElementData.DataElement != null || worldInteractableElementData.AgentState != AgentState.Move || worldInteractableElementData.TravelTime < 0) return;

        if (worldInteractableElementData.TravelTime > 0)
        {
            worldInteractableElementData.TravelTime -= counter;
            
            if(worldInteractableElementData.TravelTime < 0)
            {           
                MovementManager.Arrive(worldInteractableElementData);
            }
        }
    }

    private void CountPatience(GameWorldInteractableElementData worldInteractableElementData, float counter)
    {
        if (worldInteractableElementData.DestinationType == DestinationType.Scene   || 
            worldInteractableElementData.AgentState != AgentState.Idle              || 
            worldInteractableElementData.ActiveInteraction.CurrentPatience < 0)
        {
            return;
        }

        if (worldInteractableElementData.ActiveInteraction.InteractionDestinationDataList.Count > 1 || 
            worldInteractableElementData.ActiveInteraction.CurrentPatience > 0)
        {
            worldInteractableElementData.ActiveInteraction.CurrentPatience -= counter;

            if (worldInteractableElementData.ActiveInteraction.CurrentPatience < 0)
            {
                MovementManager.SetDestination(worldInteractableElementData);
                GameManager.instance.UpdateWorldInteractable(worldInteractableElementData);
            }
        }
    }

    private void CountInteractionDelay(float counter)
    {
        if (InteractionManager.interactionDelayTarget == null) return;

        if (InteractionManager.interactionDelay > 0)
        {
            if (!PlayerControlManager.instance.eligibleSelectionTargets.Contains(InteractionManager.interactionDelayTarget))
            {
                InteractionManager.CancelInteractionDelay();
                return;
            }

            InteractionManager.interactionDelay -= counter;
            InteractionManager.UpdateLoadingBar();

            if (InteractionManager.interactionDelay < 0)
            {
                InteractionManager.Interact();
            }
        }
    }

    private void CountSceneDuration(float counter)
    {
        if (InteractionManager.activeOutcome == null) return;

        if(InteractionManager.activeOutcome.CurrentSceneDuration >= 0)
        {
            InteractionManager.activeOutcome.CurrentSceneDuration -= counter;

            if(InteractionManager.activeOutcome.CurrentSceneDuration < 0)
            {
                ScenarioManager.instance.AllowContinue(true);
            }
        }
    }

    public void InitializeTime()
    {
        activeTime.EndTime = defaultTime;
    }

    public void InitializeGameTime(int time)
    {
        ResetInteractionTimes();

        ActiveTime = time;
    }

    public void ResetInteractionTimes()
    {
        timeEventList = new List<TimeEvent>();
    }

    public void PauseTime(bool pause)
    {
        TimeScale = pause ? 0 : activeTimeScale;
    }
    
    public void SetEditorTime(int time, bool resetEditor = false)
    {
        activeTime.EndTime = time;

        SetLighting(activeTime.EndTime);

        //No need to reload data for interactions - it's there and filtered by the organizer
        if (resetEditor)
            RenderManager.ResetPath(false);
    }

    public void SetGameTime(int time)
    {
        //Check if the active time was inbetween any event's timeframe in order to activate the event.
        //Sometimes calls this double when end time matches exactly with the active time (will match with start as well)
        timeEventList.Where(timeEvent => TimeInFrame(timeEvent.Time, activeTime.StartTime, activeTime.EndTime)).ToList().ForEach(timeEvent =>
        {
            timeEvent.WorldInteractableDataList.ForEach(worldInteractable =>
            {
                InteractionManager.CheckActorSchedule(worldInteractable);
                GameManager.instance.ResetInteractable(worldInteractable);
            });
        });
    }
    
    public void SetLighting()
    {
        SetLighting(activeTime.EndTime);
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

                timeFrameList = dataController.Data.dataList.Cast<AtmosphereElementData>().Where(x => !x.Default).Select(x => new TimeFrame() { StartTime = x.StartTime, EndTime = x.EndTime }).ToList();

                break;

            case Enums.DataType.Interaction:
                
                timeFrameList = dataController.Data.dataList.Cast<InteractionElementData>().Where(x => !x.Default).Select(x => new TimeFrame() { StartTime = x.StartTime, EndTime = x.EndTime }).ToList();
        
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
            {
                return true;
            }

        } else {

            if(time <= endTime && time >= 0 || time >= startTime && time <= hoursInDay * secondsInHour)
            {
                return true;
            }
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

                dataList = dataController.Data.dataList.Cast<AtmosphereElementData>()
                                                       .Where(x => x.Id != -1 && x.Id != changedData.Id && !x.Default)
                                                       .Select(x => new TimeFrame()
                                                       {
                                                           StartTime = x.StartTime,
                                                           EndTime = x.EndTime

                                                       }).ToList();

                var atmosphereData = (AtmosphereElementData)changedData;

                changedTimeFrame = new TimeFrame() { StartTime = atmosphereData.StartTime, EndTime = atmosphereData.EndTime };

                break;

            case Enums.DataType.Interaction:

                dataList = dataController.Data.dataList.Cast<InteractionElementData>()
                                                       .Where(x => x.Id != -1 && x.Id != changedData.Id && !x.Default)
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

    static public void AddTimeEvent(GameWorldInteractableElementData worldInteractableData)
    {
        //Collect the interaction times of all non-default interactions
        var interactionTimeList = worldInteractableData.InteractionDataList.Where(x => !x.Default).Select(x => new { StartTime = x.StartTime, EndTime = x.EndTime + 1 }).ToList();

        interactionTimeList.ForEach(interactionTime => {

            //Add new time event to list if time does not yet exist
            if (!timeEventList.Select(timeEvent => timeEvent.Time).Contains(interactionTime.StartTime))
            {
                timeEventList.Add(new TimeEvent() { Time = interactionTime.StartTime });
            }

            if (!timeEventList.Select(timeEvent => timeEvent.Time).Contains(interactionTime.EndTime))
            {
                timeEventList.Add(new TimeEvent() { Time = interactionTime.EndTime });
            }
        });

        //Find the event where one of the collection's times contains the event time and add the interactable to that event
        timeEventList.Where(timeEvent => interactionTimeList.Select(interactionTime => interactionTime.StartTime).Contains(timeEvent.Time) || 
                                         interactionTimeList.Select(interactionTime => interactionTime.EndTime).Contains(timeEvent.Time)).ToList()
                     .ForEach(x => x.WorldInteractableDataList.Add(worldInteractableData));
    }
}
