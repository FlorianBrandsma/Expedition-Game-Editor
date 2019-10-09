using UnityEngine;
using System.Collections.Generic;

public class InteractionSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchParameters = new Search.Interaction();

        //If a sceneInteractable is selected without being directly related to an objective, don't try to get this data
        if(SegmentController.Path.FindLastRoute(Enums.DataType.Objective) != null)
        {
            var objectiveData = (ObjectiveDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Objective).data.dataElement;
            searchParameters.objectiveId = new List<int>() { objectiveData.Id };
        }

        var sceneInteractableData = (SceneInteractableDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.SceneInteractable).data.dataElement;
        searchParameters.sceneInteractableId = new List<int>() { sceneInteractableData.Id };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
