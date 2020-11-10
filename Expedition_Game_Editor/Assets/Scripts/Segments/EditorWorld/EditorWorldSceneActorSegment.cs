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
        searchParameters.regionId = new List<int>() { path.FindLastRoute(Enums.DataType.Region).id };

        //Controllable world interactables
        searchParameters.chapterId = new List<int>() { path.FindLastRoute(Enums.DataType.Chapter).id };
        
        //Phase interactables belonging to the active quest
        searchParameters.questId = new List<int>() { path.FindLastRoute(Enums.DataType.Quest).id };

        //Temporary objective interactables
        searchParameters.objectiveId = new List<int>() { path.FindLastRoute(Enums.DataType.Objective).id };

        searchParameters.excludeId = SegmentController.DataController.Data.dataList.Cast<SceneActorElementData>().Select(x => x.WorldInteractableId).ToList();
    }

    public void SetSearchResult(IElementData elementData)
    {
        SetSearchParameters();
    }

    public void CloseSegment() { }
}