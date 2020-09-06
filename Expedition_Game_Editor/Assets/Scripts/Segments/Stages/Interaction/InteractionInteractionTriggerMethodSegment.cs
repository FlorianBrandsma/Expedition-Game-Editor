using UnityEngine;
using System.Linq;

public class InteractionInteractionTriggerMethodSegment : MonoBehaviour, ISegment
{
    public ExToggle triggerAutomaticallyToggle;
    public ExToggle beNearDestinationToggle;
    public ExToggle faceAgentToggle;
    public ExToggle facePartyLeaderToggle;
    public ExToggle hideInteractionIndicatorToggle;

    private bool triggerAutomatically;
    private bool beNearDestination;
    private bool faceAgent;
    private bool facePartyLeader;
    private bool hideInteractionIndicator;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public bool TriggerAutomatically
    {
        get { return triggerAutomatically; }
        set
        {
            triggerAutomatically = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.TriggerAutomatically = triggerAutomatically;
            });
        }
    }

    public bool BeNearDestination
    {
        get { return beNearDestination; }
        set
        {
            beNearDestination = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.BeNearDestination = beNearDestination;
            });
        }
    }

    public bool FaceAgent
    {
        get { return faceAgent; }
        set
        {
            faceAgent = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.FaceAgent = faceAgent;
            });
        }
    }

    public bool FacePartyLeader
    {
        get { return facePartyLeader; }
        set
        {
            facePartyLeader = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.FacePartyLeader = facePartyLeader;
            });
        }
    }

    public bool HideInteractionIndicator
    {
        get { return hideInteractionIndicator; }
        set
        {
            hideInteractionIndicator = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.HideInteractionIndicator = hideInteractionIndicator;
            });
        }
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var interactionData = (InteractionElementData)DataEditor.ElementData;

        triggerAutomatically = interactionData.TriggerAutomatically;
        beNearDestination = interactionData.BeNearDestination;
        faceAgent = interactionData.FaceAgent;
        facePartyLeader = interactionData.FacePartyLeader;
        hideInteractionIndicator = interactionData.HideInteractionIndicator;
    }

    public void InitializeSegment() { }
    
    public void UpdateTriggerAutomatically()
    {
        TriggerAutomatically = triggerAutomaticallyToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateBeNearDestination()
    {
        BeNearDestination = beNearDestinationToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateFaceAgent()
    {
        FaceAgent = faceAgentToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateFacePartyLeader()
    {
        FacePartyLeader = facePartyLeaderToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateHideInteractionIndicator()
    {
        HideInteractionIndicator = hideInteractionIndicatorToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void OpenSegment()
    {
        triggerAutomaticallyToggle.Toggle.isOn = triggerAutomatically;
        beNearDestinationToggle.Toggle.isOn = beNearDestination;
        faceAgentToggle.Toggle.isOn = faceAgent;
        facePartyLeaderToggle.Toggle.isOn = facePartyLeader;
        hideInteractionIndicatorToggle.Toggle.isOn = hideInteractionIndicator;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
