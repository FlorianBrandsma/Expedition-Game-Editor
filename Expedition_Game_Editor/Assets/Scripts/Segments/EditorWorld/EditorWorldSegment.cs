using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EditorWorldSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.EditorWorld);

        var searchParameters = searchProperties.searchParameters.Cast<Search.EditorWorld>().First();

        var regionRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Region);
        searchParameters.regionType = ((RegionElementData)regionRoute.ElementData).Type;

        var regionElementData = (RegionElementData)regionRoute.ElementData;
        searchParameters.regionId = new List<int>() { regionElementData.Id };

        var objectiveRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Objective);

        if (objectiveRoute == null)
            searchParameters.objectiveId = new List<int>() { 0 };

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
