using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionEditor : MonoBehaviour, IEditor
{
    private InteractionData interactionData;

    public Data Data                                { get { return PathController.route.data; } }
    public IElementData ElementData                 { get { return PathController.route.ElementData; } }
    public IElementData EditData                    { get { return Data.dataController.Data.dataList.Where(x => x.Id == interactionData.Id).FirstOrDefault(); } }

    private PathController PathController           { get { return GetComponent<PathController>(); } }
    public List<SegmentController> EditorSegments   { get; } = new List<SegmentController>();

    public bool Loaded { get; set; }
    
    public List<IElementData> DataList
    {
        get { return new List<IElementData>() { EditData }; }
    }

    public List<IElementData> ElementDataList
    {
        get
        {
            var list = new List<IElementData>();

            DataList.ForEach(x => { if (x != null) list.Add(x); });

            return list;
        }
    }

    #region Data properties
    public int Id
    {
        get { return interactionData.Id; }
    }

    public bool Default
    {
        get { return interactionData.Default; }
    }

    public int StartTime
    {
        get { return interactionData.StartTime; }
        set
        {
            interactionData.StartTime = value;

            DataList.ForEach(x => ((InteractionElementData)x).StartTime = value);
        }
    }

    public int EndTime
    {
        get { return interactionData.EndTime; }
        set
        {
            interactionData.EndTime = value;

            DataList.ForEach(x => ((InteractionElementData)x).EndTime = value);
        }
    }

    public int ArrivalType
    {
        get { return interactionData.ArrivalType; }
        set
        {
            interactionData.ArrivalType = value;

            DataList.ForEach(x => ((InteractionElementData)x).ArrivalType = value);
        }
    }

    public bool TriggerAutomatically
    {
        get { return interactionData.TriggerAutomatically; }
        set
        {
            interactionData.TriggerAutomatically = value;

            DataList.ForEach(x => ((InteractionElementData)x).TriggerAutomatically = value);
        }
    }

    public bool BeNearDestination
    {
        get { return interactionData.BeNearDestination; }
        set
        {
            interactionData.BeNearDestination = value;

            DataList.ForEach(x => ((InteractionElementData)x).BeNearDestination = value);
        }
    }

    public bool FaceInteractable
    {
        get { return interactionData.FaceInteractable; }
        set
        {
            interactionData.FaceInteractable = value;

            DataList.ForEach(x => ((InteractionElementData)x).FaceInteractable = value);
        }
    }

    public bool FacePartyLeader
    {
        get { return interactionData.FacePartyLeader; }
        set
        {
            interactionData.FacePartyLeader = value;

            DataList.ForEach(x => ((InteractionElementData)x).FacePartyLeader = value);
        }
    }

    public bool HideInteractionIndicator
    {
        get { return interactionData.HideInteractionIndicator; }
        set
        {
            interactionData.HideInteractionIndicator = value;

            DataList.ForEach(x => ((InteractionElementData)x).HideInteractionIndicator = value);
        }
    }

    public float InteractionRange
    {
        get { return interactionData.InteractionRange; }
        set
        {
            interactionData.InteractionRange = value;

            DataList.ForEach(x => ((InteractionElementData)x).InteractionRange = value);
        }
    }

    public int DelayMethod
    {
        get { return interactionData.DelayMethod; }
        set
        {
            interactionData.DelayMethod = value;

            DataList.ForEach(x => ((InteractionElementData)x).DelayMethod = value);
        }
    }

    public int DelayDuration
    {
        get { return interactionData.DelayDuration; }
        set
        {
            interactionData.DelayDuration = value;

            DataList.ForEach(x => ((InteractionElementData)x).DelayDuration = value);
        }
    }

    public bool HideDelayIndicator
    {
        get { return interactionData.HideDelayIndicator; }
        set
        {
            interactionData.HideDelayIndicator = value;

            DataList.ForEach(x => ((InteractionElementData)x).HideDelayIndicator = value);
        }
    }

    public bool CancelDelayOnInput
    {
        get { return interactionData.CancelDelayOnInput; }
        set
        {
            interactionData.CancelDelayOnInput = value;

            DataList.ForEach(x => ((InteractionElementData)x).CancelDelayOnInput = value);
        }
    }

    public bool CancelDelayOnMovement
    {
        get { return interactionData.CancelDelayOnMovement; }
        set
        {
            interactionData.CancelDelayOnMovement = value;

            DataList.ForEach(x => ((InteractionElementData)x).CancelDelayOnMovement = value);
        }
    }

    public bool CancelDelayOnHit
    {
        get { return interactionData.CancelDelayOnHit; }
        set
        {
            interactionData.CancelDelayOnHit = value;

            DataList.ForEach(x => ((InteractionElementData)x).CancelDelayOnHit = value);
        }
    }

    public string PublicNotes
    {
        get { return interactionData.PublicNotes; }
        set
        {
            interactionData.PublicNotes = value;

            DataList.ForEach(x => ((InteractionElementData)x).PublicNotes = value);
        }
    }

    public string PrivateNotes
    {
        get { return interactionData.PrivateNotes; }
        set
        {
            interactionData.PrivateNotes = value;

            DataList.ForEach(x => ((InteractionElementData)x).PrivateNotes = value);
        }
    }

    public string ModelIconPath
    {
        get { return interactionData.ModelIconPath; }
        set
        {
            interactionData.ModelIconPath = value;

            DataList.ForEach(x => ((InteractionElementData)x).ModelIconPath = value);
        }
    }

    public string InteractableName
    {
        get { return interactionData.InteractableName; }
        set
        {
            interactionData.InteractableName = value;

            DataList.ForEach(x => ((InteractionElementData)x).InteractableName = value);
        }
    }

    public string LocationName
    {
        get { return interactionData.LocationName; }
        set
        {
            interactionData.LocationName = value;

            DataList.ForEach(x => ((InteractionElementData)x).LocationName = value);
        }
    }

    public bool TimeConflict
    {
        get { return interactionData.TimeConflict; }
        set
        {
            interactionData.TimeConflict = value;

            DataList.ForEach(x => ((InteractionElementData)x).TimeConflict = value);
        }
    }
    #endregion

    public void InitializeEditor()
    {
        interactionData = (InteractionData)ElementData.Clone();
    }

    public void OpenEditor() { }

    public void UpdateEditor()
    {
        SetEditor();
    }

    public void SetEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed) && !interactionData.TimeConflict;
    }

    public void ApplyChanges()
    {
        var changedTime =   ((InteractionElementData)EditData).ChangedStartTime ||
                            ((InteractionElementData)EditData).ChangedEndTime;

        EditData.Update();

        if (changedTime)
        {
            Data.dataList = Data.dataList.OrderByDescending(x => ((InteractionElementData)x).Default).ThenBy(x => ((InteractionElementData)x).StartTime).ToList();

            var defaultInteraction = (InteractionElementData)Data.dataList.Where(x => ((InteractionElementData)x).Default).First();
            defaultInteraction.DefaultTimes = DefaultTimes();

            SelectionElementManager.UpdateElements(ElementData);

        } else {

            if (SelectionElementManager.SelectionActive(EditData.DataElement))
                EditData.DataElement.UpdateElement();
        }

        UpdateEditor();
    }

    private List<int> DefaultTimes()
    {
        var timeFrameList = (from interactionData in Data.dataList.Cast<InteractionElementData>().Where(x => !x.Default)
                             select new TimeManager.TimeFrame()
                             {
                                 StartTime = interactionData.StartTime,
                                 EndTime = interactionData.EndTime

                             }).ToList();

        var defaultTimes = TimeManager.AvailableTimes(timeFrameList);

        return defaultTimes;
    }

    public void CancelEdit()
    {
        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
