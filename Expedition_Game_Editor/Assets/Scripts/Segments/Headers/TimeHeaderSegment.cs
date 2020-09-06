using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

public class TimeHeaderSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    #region UI
    public GameObject defaultHeader;
    public GameObject timeInput;
    public ExInputNumber startTimeInput, endTimeInput;
    public Text idText;
    #endregion

    #region Data Variables
    private int id;
    private int index;
    private bool isDefault;
    private int startTime;
    private int endTime;
    private string modelIcon;
    #endregion

    #region Data Properties    
    private int StartTime
    {
        get { return startTime; }
        set
        {
            startTime = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:

                    var atmosphereData = (AtmosphereElementData)DataEditor.ElementData;
                    atmosphereData.StartTime = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionData = (InteractionElementData)DataEditor.ElementData;
                    interactionData.StartTime = value;

                    break;
            }
        }
    }

    private int EndTime
    {
        get { return endTime; }
        set
        {
            endTime = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:

                    var atmosphereData = (AtmosphereElementData)DataEditor.ElementData;
                    atmosphereData.EndTime = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionData = (InteractionElementData)DataEditor.ElementData;
                    interactionData.EndTime = value;

                    break;
            }
        }
    }
    #endregion

    #region Methods
    public void UpdateStartTime()
    {
        StartTime = int.Parse(startTimeInput.inputField.text) * TimeManager.secondsInHour;

        CheckTime();

        DataEditor.UpdateEditor();
    }

    public void UpdateEndTime()
    {
        EndTime = int.Parse(endTimeInput.inputField.text) * TimeManager.secondsInHour + (TimeManager.secondsInHour - 1);
        
        CheckTime();

        DataEditor.UpdateEditor();
    }

    private void CheckTime()
    {
        var timeConflict = isDefault ? false : TimeManager.TimeConflict(DataEditor.Data.dataController, DataEditor.ElementData);

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Atmosphere:

                var atmosphereData = (AtmosphereElementData)DataEditor.ElementData;
                atmosphereData.TimeConflict = timeConflict;

                break;

            case Enums.DataType.Interaction:

                var interactionData = (InteractionElementData)DataEditor.ElementData;
                interactionData.TimeConflict = timeConflict;

                break;
        }
        
        startTimeInput.InputInvalid = timeConflict;
        endTimeInput.InputInvalid = timeConflict;
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Atmosphere: InitializeAtmosphereData(); break;
            case Enums.DataType.Interaction: InitializeInteractionData(); break;
        }
    }

    public void InitializeSegment()
    {
        CheckTime();
    }
    
    private void InitializeAtmosphereData()
    {
        var atmosphereData = (AtmosphereElementData)DataEditor.ElementData;

        id          = atmosphereData.Id;

        isDefault   = atmosphereData.Default;

        startTime   = atmosphereData.StartTime;
        endTime     = atmosphereData.EndTime;
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionElementData)DataEditor.ElementData;

        id          = interactionData.Id;

        isDefault   = interactionData.Default;

        startTime   = interactionData.StartTime;
        endTime     = interactionData.EndTime;
    }

    public void OpenSegment()
    {
        idText.text = id.ToString();

        defaultHeader.SetActive(isDefault);
        timeInput.SetActive(!isDefault);

        if(!isDefault)
        {
            startTimeInput.inputField.text  = TimeSpan.FromSeconds(startTime).Hours.ToString();
            endTimeInput.inputField.text    = TimeSpan.FromSeconds(endTime).Hours.ToString();
        }

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
