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

        var searchParameters = new Search.Interaction();

        //If a terrainElement is selected without being directly related to an objective, don't try to get this data
        if(SegmentController.Path.FindLastRoute(Enums.DataType.Objective) != null)
        {
            var objectiveData = (ObjectiveDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Objective).data.DataElement;
            searchParameters.objectiveId = new List<int>() { objectiveData.id };
        }

        var terrainElementData = (TerrainInteractableDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.TerrainInteractable).data.DataElement;
        searchParameters.terrainInteractableId = new List<int>() { terrainElementData.id };

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
