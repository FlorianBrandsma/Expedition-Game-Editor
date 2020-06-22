using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TaskSaveSegment : MonoBehaviour, ISegment
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

        var searchProperties = new SearchProperties(Enums.DataType.TaskSave);

        var searchParameters = searchProperties.searchParameters.Cast<Search.TaskSave>().First();

        //If a worldInteractable is selected without being directly related to an objective, don't try to get this data
        if (SegmentController.Path.FindLastRoute(Enums.DataType.ObjectiveSave) != null)
        {
            var objectiveSaveData = (ObjectiveSaveDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.ObjectiveSave).data.dataElement;
            searchParameters.objectiveSaveId = new List<int>() { objectiveSaveData.Id };
        }

        var worldInteractableData = (WorldInteractableDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.WorldInteractable).data.dataElement;
        searchParameters.worldInteractableId = new List<int>() { worldInteractableData.Id };

        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement) { }
}
