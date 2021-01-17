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

    public bool Loaded                              { get; set; }
    public bool Removable                           { get { return !interactionData.Default; } }

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

    public bool FaceControllable
    {
        get { return interactionData.FaceControllable; }
        set
        {
            interactionData.FaceControllable = value;

            DataList.ForEach(x => ((InteractionElementData)x).FaceControllable = value);
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

    public float DelayDuration
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

    public void ResetEditor() { }

    public void UpdateEditor()
    {
        PathController.layoutSection.SetActionButtons();
    }

    public bool Changed()
    {
        return ElementDataList.Any(x => x.Changed);
    }

    public void ApplyChanges(DataRequest dataRequest)
    {
        ApplyInteractionChanges(dataRequest);
    }

    private void ApplyInteractionChanges(DataRequest dataRequest)
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
                AddInteraction(dataRequest);
                break;

            case Enums.ExecuteType.Update:
                UpdateInteraction(dataRequest);
                break;

            case Enums.ExecuteType.Remove:
                RemoveInteraction(dataRequest);
                break;
        }
    }

    private void AddInteraction(DataRequest dataRequest)
    {
        var tempData = EditData;

        EditData.Add(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
            interactionData.Id = tempData.Id;
    }

    private void UpdateInteraction(DataRequest dataRequest)
    {
        var changedTime = ((InteractionElementData)EditData).ChangedStartTime ||
                          ((InteractionElementData)EditData).ChangedEndTime;

        EditData.Update(dataRequest);

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (changedTime)
            {
                Data.dataController.Data.dataList = Data.dataController.Data.dataList.OrderByDescending(x => x.ExecuteType == Enums.ExecuteType.Add)
                                                                                     .ThenBy(x => !((InteractionElementData)x).Default)
                                                                                     .ThenBy(x => ((InteractionElementData)x).StartTime).ToList();

                var defaultInteraction = (InteractionElementData)Data.dataController.Data.dataList.Where(x => ((InteractionElementData)x).Default).First();
                defaultInteraction.DefaultTimes = DefaultTimes();

                SelectionElementManager.UpdateElements(ElementData);

            } else {

                if (SelectionElementManager.SelectionActive(EditData.DataElement))
                    EditData.DataElement.UpdateElement();
            }
        }
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

    private void RemoveInteraction(DataRequest dataRequest)
    {
        EditData.Remove(dataRequest);
    }
    
    public void FinalizeChanges()
    {
        switch (EditData.ExecuteType)
        {
            case Enums.ExecuteType.Add:
            case Enums.ExecuteType.Remove:
                RenderManager.PreviousPath();
                break;
            case Enums.ExecuteType.Update:
                ResetExecuteType();
                UpdateEditor();
                break;
        }
    }

    private void ResetExecuteType()
    {
        ElementDataList.Where(x => x.Id != -1).ToList().ForEach(x => x.ExecuteType = Enums.ExecuteType.Update);
    }

    public void CancelEdit()
    {
        ResetExecuteType();

        ElementDataList.ForEach(x => x.ClearChanges());

        Loaded = false;
    }

    public void CloseEditor() { }
}
