using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EditorWorldScenePropSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.SceneProp);

        var searchParameters = searchProperties.searchParameters.Cast<Search.SceneProp>().First();
        searchParameters.sceneId = new List<int>() { RenderManager.layoutManager.forms.First().activePath.FindLastRoute(Enums.DataType.Scene).ElementData.Id };

        //List and show (on the world) all props that belong to the scene. It is assumed the position is meant for the scene region

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
        var searchParameters = SegmentController.DataController.SearchProperties.searchParameters.Cast<Search.Model>().First();
        searchParameters.excludeId = SegmentController.DataController.Data.dataList.Cast<ScenePropElementData>().Select(x => x.ModelId).ToList();
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData)
    {
        SetSearchParameters();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}