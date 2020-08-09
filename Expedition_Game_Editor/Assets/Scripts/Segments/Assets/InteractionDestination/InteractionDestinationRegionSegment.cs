using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionDestinationRegionSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var interactionDestinationData = (InteractionDestinationElementData)SegmentController.Path.FindLastRoute(Enums.DataType.InteractionDestination).data.elementData;
        SegmentController.ListProperties.autoSelectId = interactionDestinationData.RegionId;
        
        var searchProperties = new SearchProperties(Enums.DataType.Region);
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        var phaseId = 0;

        if (SegmentController.Path.FindLastRoute(Enums.DataType.Phase) != null)
            phaseId = SegmentController.Path.FindLastRoute(Enums.DataType.Phase).GeneralData.Id;
        
        searchParameters.phaseId = new List<int>() { phaseId };

        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
