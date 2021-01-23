using UnityEngine;
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

        InitializeSearchParameters(searchProperties);

        SegmentController.DataController.GetData(searchProperties);
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.EditorWorld>().First();

        var regionRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Region);
        searchParameters.regionType = ((RegionElementData)regionRoute.ElementData).Type;

        var regionElementData = (RegionElementData)regionRoute.ElementData;
        searchParameters.regionId = new List<int>() { regionElementData.Id };

        var objectiveRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Objective);

        if (objectiveRoute == null)
            searchParameters.objectiveId = new List<int>() { 0 };

        var interactionRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Interaction);

        if (interactionRoute != null)
            searchParameters.interactionId = new List<int>() { interactionRoute.id };

        var sceneRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Scene);

        if (sceneRoute != null)
            searchParameters.sceneId = new List<int>() { sceneRoute.id };

        switch(searchParameters.regionType)
        {
            case Enums.RegionType.Base:
            case Enums.RegionType.Phase:
                searchParameters.includeAddWorldObjectElement = true;
                break;
            case Enums.RegionType.InteractionDestination:
                searchParameters.includeAddInteractionDestinationElement = true;
                break;
            case Enums.RegionType.Scene:
                searchParameters.includeAddSceneActorElement = true;
                searchParameters.includeAddScenePropElement = true;
                break;
        }
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

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
