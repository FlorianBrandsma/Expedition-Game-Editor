using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class TaskSegment : MonoBehaviour, ISegment
{
    public ListProperties ListProperties        { get { return GetComponent<ListProperties>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }
    
    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.Task);

        InitializeSearchParameters(searchProperties);
        
        SegmentController.DataController.GetData(searchProperties);
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Task>().First();

        searchParameters.includeAddElement = ListProperties.AddProperty != SelectionManager.Property.None;

        //If a worldInteractable is selected without being directly related to an objective, don't try to get this data
        if (SegmentController.Path.FindLastRoute(Enums.DataType.Objective) != null)
        {
            var objectiveData = (ObjectiveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Objective).ElementData;
            searchParameters.objectiveId = new List<int>() { objectiveData.Id };
        }

        var worldInteractableData = (WorldInteractableElementData)SegmentController.Path.FindLastRoute(Enums.DataType.WorldInteractable).ElementData;
        searchParameters.worldInteractableId = new List<int>() { worldInteractableData.Id };
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
