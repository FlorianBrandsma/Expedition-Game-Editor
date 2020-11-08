using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterCharactersControllableAgentSegment : MonoBehaviour, ISegment
{
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

        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();
        searchParameters.chapterId = new List<int>() { ChapterEditor.Id };

        SegmentController.DataController.GetData(searchProperties);

        var worldInteractableList = SegmentController.DataController.Data.dataList.Cast<WorldInteractableElementData>().ToList();
        worldInteractableList.ForEach(x => ChapterEditor.worldInteractableElementDataList.Add(x));
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
    }

    public void SetSearchResult(IElementData elementData)
    {
        SetSearchWorldInteractable((WorldInteractableElementData)elementData);

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }

    private void SetSearchWorldInteractable(WorldInteractableElementData resultElementData)
    {
        var elementData = ChapterEditor.worldInteractableElementDataList.Where(x => x.Id == resultElementData.Id).First();

        elementData.InteractableId      = resultElementData.InteractableId;
        elementData.InteractableName    = resultElementData.InteractableName;
        elementData.ModelIconPath       = resultElementData.ModelIconPath;
    }

    public void CloseSegment() { }
}
