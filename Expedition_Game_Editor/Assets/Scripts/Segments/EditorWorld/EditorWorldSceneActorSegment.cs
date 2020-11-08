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

        //Objective interactables and region interactables can be picked as actors
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

    public void CloseSegment() { }
}