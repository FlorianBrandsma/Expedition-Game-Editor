using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterCharactersPersistentInteractableSegment : MonoBehaviour, ISegment
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

        var searchProperties = new SearchProperties(Enums.DataType.ChapterInteractable);

        InitializeSearchParameters(searchProperties);

        SegmentController.DataController.GetData(searchProperties);

        var chapterInteractableList = SegmentController.DataController.Data.dataList.Cast<ChapterInteractableElementData>().ToList();
        ChapterEditor.chapterInteractableElementDataList = chapterInteractableList;
    }

    private void InitializeSearchParameters(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterInteractable>().First();

        searchParameters.includeAddElement = ListProperties.AddProperty != SelectionManager.Property.None;
        searchParameters.chapterId = new List<int>() { ChapterEditor.Id };
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
        SetSearchChapterInteractable((ChapterInteractableElementData)mergedElementData, resultElementData);

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }

    private void SetSearchChapterInteractable(ChapterInteractableElementData mergedElementData, IElementData resultElementData)
    {
        var searchElementData = (ChapterInteractableElementData)SegmentController.DataController.Data.dataList.Where(x => x.Id == mergedElementData.Id).First();

        if (searchElementData.Id == 0)
        {
            searchElementData = (ChapterInteractableElementData)searchElementData.Clone();

            searchElementData.ExecuteType = Enums.ExecuteType.Add;

            searchElementData.Id = -ChapterEditor.worldInteractableElementDataList.Count;

            searchElementData.ChapterId         = ChapterEditor.EditData.Id;
            searchElementData.InteractableId    = mergedElementData.InteractableId;

            searchElementData.InteractableName  = mergedElementData.InteractableName;
            searchElementData.ModelIconPath     = mergedElementData.ModelIconPath;

            SegmentController.DataController.Data.dataList.Add(searchElementData);
            ChapterEditor.chapterInteractableElementDataList.Add(searchElementData);
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
