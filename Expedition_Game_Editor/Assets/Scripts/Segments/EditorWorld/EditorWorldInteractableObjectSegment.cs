using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EditorWorldInteractableObjectSegment : MonoBehaviour, ISegment
{
    public ListProperties ListProperties        { get { return GetComponent<ListProperties>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.WorldInteractable);

        InitializeSearchParameters(searchProperties);

        SegmentController.DataController.GetData(searchProperties);
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        searchParameters.requestType = Search.WorldInteractable.RequestType.GetRegionWorldInteractables;

        searchParameters.includeAddElement = ListProperties.AddProperty != SelectionManager.Property.None;

        searchParameters.type           = new List<int>() { (int)Enums.InteractableType.Object };

        searchParameters.regionId       = new List<int>() { RenderManager.layoutManager.forms.First().activePath.FindLastRoute(Enums.DataType.Region).ElementData.Id };
        searchParameters.objectiveId    = new List<int>() { 0 };
    }

    public void InitializeSegment()
    {
        InitializeData();
    }
    
    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    private void SetSearchParameters()
    {
        var searchParameters = SegmentController.DataController.SearchProperties.searchParameters.Cast<Search.Interactable>().First();

        searchParameters.includeRemoveElement = true;
        searchParameters.type = new List<int>() { (int)Enums.InteractableType.Object };
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData)
    {
        if (mergedElementData.ExecuteType == Enums.ExecuteType.Add || resultElementData.Id == 0)
            RenderManager.ResetPath(true);
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
