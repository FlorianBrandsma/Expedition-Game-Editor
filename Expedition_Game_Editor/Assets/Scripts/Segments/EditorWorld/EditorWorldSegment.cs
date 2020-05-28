using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EditorWorldSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.EditorWorld);

        var searchParameters = searchProperties.searchParameters.Cast<Search.EditorWorld>().First();

        var regionData = SegmentController.Path.FindLastRoute(Enums.DataType.Region).data;
        searchParameters.regionType = ((RegionController)regionData.dataController).regionType;

        var regionDataElement = (RegionDataElement)regionData.dataElement;
        searchParameters.regionId = new List<int>() { regionDataElement.Id };
        
        var objectiveRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Objective);

        if (objectiveRoute == null)
            searchParameters.objectiveId = new List<int>() { 0 };

        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
