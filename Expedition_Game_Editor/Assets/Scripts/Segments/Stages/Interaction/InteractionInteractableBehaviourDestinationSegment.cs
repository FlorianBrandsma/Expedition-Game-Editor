using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionInteractableBehaviourDestinationSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private InteractionEditor InteractionEditor { get { return (InteractionEditor)DataEditor; } }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.InteractionDestination);

        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionDestination>().First();
        searchParameters.interactionId = new List<int>() { InteractionEditor.Id };

        SegmentController.DataController.GetData(searchProperties);
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void CloseSegment() { }
}
