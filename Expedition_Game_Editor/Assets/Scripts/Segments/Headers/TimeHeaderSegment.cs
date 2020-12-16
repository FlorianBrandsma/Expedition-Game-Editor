using UnityEngine;
using UnityEngine.UI;
using System;

public class TimeHeaderSegment : MonoBehaviour, ISegment
{
    public GameObject defaultHeader;
    public GameObject timeInput;
    public ExInputNumber startTimeInput, endTimeInput;
    public Text idText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region Data properties
    public int Id
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:
                    return ((AtmosphereEditor)DataEditor).Id;

                case Enums.DataType.Interaction:
                    return ((InteractionEditor)DataEditor).Id;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
    }

    public bool Default
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:
                    return ((AtmosphereEditor)DataEditor).Default;

                case Enums.DataType.Interaction:
                    return ((InteractionEditor)DataEditor).Default;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return false; }
            }
        }
    }

    private int StartTime
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:
                    return ((AtmosphereEditor)DataEditor).StartTime;

                case Enums.DataType.Interaction:
                    return ((InteractionEditor)DataEditor).StartTime;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:

                    var atmosphereEditor = (AtmosphereEditor)DataEditor;
                    atmosphereEditor.StartTime = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionEditor = (InteractionEditor)DataEditor;
                    interactionEditor.StartTime = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private int EndTime
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:
                    return ((AtmosphereEditor)DataEditor).EndTime;

                case Enums.DataType.Interaction:
                    return ((InteractionEditor)DataEditor).EndTime;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return 0; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:

                    var atmosphereEditor = (AtmosphereEditor)DataEditor;
                    atmosphereEditor.EndTime = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionEditor = (InteractionEditor)DataEditor;
                    interactionEditor.EndTime = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    private bool TimeConflict
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:
                    return ((AtmosphereEditor)DataEditor).TimeConflict;

                case Enums.DataType.Interaction:
                    return ((InteractionEditor)DataEditor).TimeConflict;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return false; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:

                    var atmosphereEditor = (AtmosphereEditor)DataEditor;
                    atmosphereEditor.TimeConflict = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionEditor = (InteractionEditor)DataEditor;
                    interactionEditor.TimeConflict = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }
    #endregion

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
    }

    public void InitializeSegment()
    {
        CheckTime();
    }

    private void CheckTime()
    {
        TimeConflict = Default ? false : TimeManager.TimeConflict(DataEditor.Data.dataController, DataEditor.EditData);

        startTimeInput.InputInvalid = TimeConflict;
        endTimeInput.InputInvalid = TimeConflict;
    }

    public void OpenSegment()
    {
        idText.text = Id.ToString();

        defaultHeader.SetActive(Default);
        timeInput.SetActive(!Default);

        if(!Default)
        {
            startTimeInput.inputField.text  = TimeSpan.FromSeconds(StartTime).Hours.ToString();
            endTimeInput.inputField.text    = TimeSpan.FromSeconds(EndTime).Hours.ToString();
        }

        gameObject.SetActive(true);
    }

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
    
    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }
}
