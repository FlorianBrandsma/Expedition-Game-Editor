using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveGeneralInteractableSegment : MonoBehaviour, ISegment
{
    public ListProperties ListProperties        { get { return GetComponent<ListProperties>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private ObjectiveEditor ObjectiveEditor     { get { return (ObjectiveEditor)DataEditor; } }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }
    
    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.WorldInteractable);

        InitializeSearchParameters(searchProperties);

        SegmentController.DataController.GetData(searchProperties);

        var worldInteractableList = SegmentController.DataController.Data.dataList.Cast<WorldInteractableElementData>().ToList();
        ObjectiveEditor.worldInteractableElementDataList = worldInteractableList.Where(x => x.Id != -1).ToList();

        SetSearchParameters();
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        searchParameters.includeAddElement = ListProperties.AddProperty != SelectionManager.Property.None;

        searchParameters.objectiveId    = new List<int>() { ObjectiveEditor.Id };
        searchParameters.type           = new List<int>() { (int)Enums.InteractableType.Object };
    }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
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
        SetSearchWorldInteractable((WorldInteractableElementData)mergedElementData, resultElementData);

        DataEditor.UpdateEditor();
    }

    private void SetSearchWorldInteractable(WorldInteractableElementData mergedElementData, IElementData resultElementData)
    {
        var searchElementData = (WorldInteractableElementData)SegmentController.DataController.Data.dataList.Where(x => x.Id == mergedElementData.Id).First();

        if (searchElementData.Id == -1)
        {
            searchElementData = (WorldInteractableElementData)searchElementData.Clone();

            searchElementData.ExecuteType = Enums.ExecuteType.Add;

            searchElementData.Id = -(ObjectiveEditor.worldInteractableElementDataList.Count + 2);

            searchElementData.ObjectiveId       = ObjectiveEditor.EditData.Id;
            searchElementData.InteractableId    = mergedElementData.InteractableId;
            searchElementData.Type              = (int)Enums.InteractableType.Object;

            searchElementData.InteractableName  = mergedElementData.InteractableName;
            searchElementData.ModelIconPath     = mergedElementData.ModelIconPath;

            SegmentController.DataController.Data.dataList.Add(searchElementData);
            ObjectiveEditor.worldInteractableElementDataList.Add(searchElementData);
        }

        if (resultElementData.Id == 0)
        {
            searchElementData.ExecuteType = Enums.ExecuteType.Remove;

            SegmentController.DataController.Data.dataList.Remove(searchElementData);
        }
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
