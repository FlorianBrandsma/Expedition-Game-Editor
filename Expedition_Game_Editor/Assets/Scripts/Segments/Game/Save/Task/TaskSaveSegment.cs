using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TaskSaveSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.TaskSave);

        var searchParameters = searchProperties.searchParameters.Cast<Search.TaskSave>().First();

        //Don't try to get this data if a worldInteractable is selected without being directly related to an objective
        if (SegmentController.Path.FindLastRoute(Enums.DataType.ObjectiveSave) != null)
        {
            var objectiveSaveElementData = (ObjectiveSaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.ObjectiveSave).ElementData;

            searchParameters.objectiveId = new List<int>() { objectiveSaveElementData.ObjectiveId };
        }

        var worldInteractableElementData = (WorldInteractableElementData)SegmentController.Path.FindLastRoute(Enums.DataType.WorldInteractable).ElementData;
        searchParameters.worldInteractableId = new List<int>() { worldInteractableElementData.Id };

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

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
