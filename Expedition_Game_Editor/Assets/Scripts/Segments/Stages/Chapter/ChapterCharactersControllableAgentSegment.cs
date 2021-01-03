using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterCharactersControllableAgentSegment : MonoBehaviour, ISegment
{
    public ListProperties ListProperties        { get { return GetComponent<ListProperties>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private ChapterEditor ChapterEditor         { get { return (ChapterEditor)DataEditor; } }

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
        ChapterEditor.worldInteractableElementDataList = worldInteractableList.Where(x => x.Id != 0).ToList();
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();
        
        searchParameters.type       = new List<int>() { (int)Enums.InteractableType.Controllable };
        searchParameters.chapterId  = new List<int>() { ChapterEditor.Id };

        searchParameters.includeAddElement = ListProperties.AddProperty != SelectionManager.Property.None;
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    private void SetSearchParameters()
    {
        var searchParameters = SegmentController.DataController.SearchProperties.searchParameters.Cast<Search.Interactable>().First();

        searchParameters.excludeId = ChapterEditor.worldInteractableElementDataList.Select(x => x.InteractableId).Union(
                                     ChapterEditor.chapterInteractableElementDataList.Select(x => x.InteractableId)).Distinct().ToList();

        searchParameters.includeRemoveElement = true;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData)
    {
        SetSearchWorldInteractable((WorldInteractableElementData)mergedElementData, resultElementData);

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }

    private void SetSearchWorldInteractable(WorldInteractableElementData mergedElementData, IElementData resultElementData)
    {
        var searchElementData = (WorldInteractableElementData)SegmentController.DataController.Data.dataList.Where(x => x.Id == mergedElementData.Id).First();

        if (searchElementData.Id == 0) 
        {
            searchElementData = (WorldInteractableElementData)searchElementData.Clone();

            searchElementData.ExecuteType = Enums.ExecuteType.Add;

            searchElementData.Id = -(ChapterEditor.worldInteractableElementDataList.Count + 1);

            searchElementData.ChapterId         = ChapterEditor.EditData.Id;
            searchElementData.InteractableId    = mergedElementData.InteractableId;
            searchElementData.Type              = (int)Enums.InteractableType.Controllable;

            searchElementData.InteractableName  = mergedElementData.InteractableName;
            searchElementData.ModelIconPath     = mergedElementData.ModelIconPath;

            SegmentController.DataController.Data.dataList.Add(searchElementData);
            ChapterEditor.worldInteractableElementDataList.Add(searchElementData);
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
