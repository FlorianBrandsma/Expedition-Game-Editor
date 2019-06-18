using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TaskSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        var searchParameters = new Search.Task();

        //If a terrainElement is selected without being directly related to an objective, don't try to get this data
        var objectiveData = (ObjectiveDataElement)SegmentController.Path.FindLastRoute("Objective").data.DataElement;
        searchParameters.objectiveId = new List<int>() { objectiveData.id };

        var terrainElementData = (TerrainElementDataElement)SegmentController.Path.FindLastRoute("TerrainElement").data.DataElement;
        searchParameters.terrainElementId = new List<int>() { terrainElementData.id };

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
