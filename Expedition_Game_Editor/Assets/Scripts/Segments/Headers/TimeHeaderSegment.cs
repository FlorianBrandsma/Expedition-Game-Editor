﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class TimeHeaderSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public GameObject defaultHeader;
    public GameObject timeInput;
    public EditorInputNumber startTimeInput, endTimeInput;
    public Text idText;
    #endregion

    #region Data Variables
    private int id;
    private int index;
    private bool isDefault;
    private int startTime;
    private int endTime;
    private string objectGraphicIcon;
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

                    var atmosphereData = (AtmosphereDataElement)DataEditor.Data.dataElement;
                    atmosphereData.StartTime = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;
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

                    var atmosphereData = (AtmosphereDataElement)DataEditor.Data.dataElement;
                    atmosphereData.EndTime = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;
                    interactionData.EndTime = value;

                    break;
            }
        }
    }
    #endregion

    #region Methods
    public void UpdateStartTime()
    {
        StartTime = int.Parse(startTimeInput.inputField.text);

        CheckTime();

        DataEditor.UpdateEditor();
    }

    public void UpdateEndTime()
    {
        EndTime = int.Parse(endTimeInput.inputField.text);

        CheckTime();

        DataEditor.UpdateEditor();
    }

    private void CheckTime()
    {
        var timeConflict = TimeManager.TimeConflict(DataEditor.Data.dataController, DataEditor.Data.dataElement);

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Atmosphere:

                var atmosphereData = (AtmosphereDataElement)DataEditor.Data.dataElement;
                atmosphereData.timeConflict = timeConflict;

                break;

            case Enums.DataType.Interaction:

                var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;
                interactionData.timeConflict = timeConflict;

                break;
        }
        
        startTimeInput.InputInvalid = timeConflict;
        endTimeInput.InputInvalid = timeConflict;
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        CheckTime();
    }

    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Atmosphere:     InitializeAtmosphereData();     break;
            case Enums.DataType.Interaction:    InitializeInteractionData();    break;
        } 
    }

    private void InitializeAtmosphereData()
    {
        var atmosphereData = (AtmosphereDataElement)DataEditor.Data.dataElement;

        id          = atmosphereData.Id;
        index       = atmosphereData.Index;

        isDefault   = atmosphereData.Default;

        startTime   = atmosphereData.StartTime;
        endTime     = atmosphereData.EndTime;
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        id          = interactionData.Id;
        index       = interactionData.Index;

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
            startTimeInput.inputField.text = startTime.ToString();
            endTimeInput.inputField.text = endTime.ToString();
        }

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}