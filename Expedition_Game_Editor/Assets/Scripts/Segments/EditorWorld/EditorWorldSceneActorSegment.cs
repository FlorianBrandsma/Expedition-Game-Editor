using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EditorWorldSceneActorSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.SceneActor);

        var searchParameters = searchProperties.searchParameters.Cast<Search.SceneActor>().First();
        searchParameters.sceneId = new List<int>() { RenderManager.layoutManager.forms.First().activePath.FindLastRoute(Enums.DataType.Scene).ElementData.Id };

        //List all actors but only show (on the world) those where ChangePosition is checked. It is assumed the position is meant for the scene region
        //Maybe add a status somewhere, whether their region is unknown or not (technically only unknown if they have destinations in multiple regions...)

        SegmentController.DataController.GetData(searchProperties);

        SetSearchParameters();
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

    private void SetSearchParameters()
    {
        var path = RenderManager.layoutManager.forms.First().activePath;

        var searchParameters = SegmentController.DataController.SearchProperties.searchParameters.Cast<Search.WorldInteractable>().First();
        searchParameters.requestType = Search.WorldInteractable.RequestType.GetSceneActorWorldInteractables;

        //Region world interactables (all phase regions)
        var regionRoute = path.FindLastRoute(Enums.DataType.Region);

        if (regionRoute != null)
            searchParameters.regionId = new List<int>() { regionRoute.id };

        //Controllable world interactables
        var chapterRoute = path.FindLastRoute(Enums.DataType.Chapter);

        if (chapterRoute != null)
            searchParameters.chapterId = new List<int>() { chapterRoute.id };

        //Phase interactables belonging to the active quest
        var questRoute = path.FindLastRoute(Enums.DataType.Quest);

        if(questRoute != null)
            searchParameters.questId = new List<int>() { questRoute.id };

        //Temporary objective interactables
        var objectiveRoute = path.FindLastRoute(Enums.DataType.Objective);

        if (objectiveRoute != null)
            searchParameters.objectiveId = new List<int>() { objectiveRoute.id };

        searchParameters.excludeId = SegmentController.DataController.Data.dataList.Cast<SceneActorElementData>().Select(x => x.WorldInteractableId).ToList();
    }

    public void SetSearchResult(IElementData elementData)
    {
        SetSearchParameters();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}