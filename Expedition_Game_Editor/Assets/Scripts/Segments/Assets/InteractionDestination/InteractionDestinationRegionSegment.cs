using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionDestinationRegionSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }
    
    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var interactionDestinationElementData = (InteractionDestinationElementData)SegmentController.Path.FindLastRoute(Enums.DataType.InteractionDestination).ElementData;
        SegmentController.ListProperties.autoSelectId = interactionDestinationElementData.RegionId;
        
        var searchProperties = new SearchProperties(Enums.DataType.Region);
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        var phaseId = 0;

        if (SegmentController.Path.FindLastRoute(Enums.DataType.Phase) != null)
            phaseId = SegmentController.Path.FindLastRoute(Enums.DataType.Phase).ElementData.Id;
        
        searchParameters.phaseId = new List<int>() { phaseId };
        searchParameters.type = Enums.RegionType.InteractionDestination;

        SegmentController.DataController.GetData(searchProperties);
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
