using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SceneShotSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;
        
        var searchProperties = new SearchProperties(Enums.DataType.SceneShot);
        var searchParameters = searchProperties.searchParameters.Cast<Search.SceneShot>().First();

        var sceneId = 0;

        var sceneRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Scene);

        if (sceneRoute != null)
            sceneId = sceneRoute.ElementData.Id;

        searchParameters.sceneId = new List<int>() { sceneId };

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

    public void CloseSegment() { }
}
