using UnityEngine;
using System.Collections.Generic;

public class TaskSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchParameters = new Search.Task();

        //If a worldInteractable is selected without being directly related to an objective, don't try to get this data
        if (SegmentController.Path.FindLastRoute(Enums.DataType.Objective) != null)
        {
            var objectiveData = (ObjectiveDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Objective).data.dataElement;
            searchParameters.objectiveId = new List<int>() { objectiveData.Id };
        }

        var worldInteractableData = (WorldInteractableDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.WorldInteractable).data.dataElement;
        searchParameters.worldInteractableId = new List<int>() { worldInteractableData.Id };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
