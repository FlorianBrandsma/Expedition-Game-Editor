using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.editorController.PathController.loaded) return;

        var searchParameters = new Search.Scene();
        
        var regionDataElement = (RegionDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.dataElement;
        searchParameters.regionId = new List<int>() { regionDataElement.Id };
        
        var objectiveRoute = SegmentController.Path.FindLastRoute(Enums.DataType.Objective);

        if (objectiveRoute == null)
            searchParameters.objectiveId = new List<int>() { 0 };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });

        regionDataElement.sceneDataElement = SegmentController.DataController.DataList.Cast<SceneDataElement>().FirstOrDefault();
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        selectionElement.data.dataElement.Update();
    }
}
