using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterCharactersConstantInteractableSegment : MonoBehaviour, ISegment
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

        var searchProperties = new SearchProperties(Enums.DataType.ChapterInteractable);

        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterInteractable>().First();
        searchParameters.chapterId = new List<int>() { ChapterEditor.Id };

        SegmentController.DataController.GetData(searchProperties);

        var chapterInteractableList = SegmentController.DataController.Data.dataList.Cast<ChapterInteractableElementData>().ToList();
        chapterInteractableList.ForEach(x => ChapterEditor.chapterInteractableElementDataList.Add(x));
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
        var searchProperties = SegmentController.DataController.SearchProperties;

        var searchParameters = searchProperties.searchParameters.Cast<Search.Interactable>().First();

        var idList = ChapterEditor.partyMemberElementDataList.Select(x => x.InteractableId).Union(
                     ChapterEditor.chapterInteractableElementDataList.Select(x => x.InteractableId)).Distinct().ToList();

        //Find interactables where id is not in the list
        var list = DataManager.GetInteractableData().Where(x => !idList.Contains(x.Id)).Select(x => x.Id).Distinct().ToList();

        searchParameters.id = list;
    }

    public void SetSearchResult(IElementData elementData)
    {
        SetSearchChapterInteractable((ChapterInteractableElementData)elementData);

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }

    private void SetSearchChapterInteractable(ChapterInteractableElementData resultElementData)
    {
        var elementData = ChapterEditor.chapterInteractableElementDataList.Where(x => x.Id == resultElementData.Id).First();

        elementData.InteractableId      = resultElementData.InteractableId;
        elementData.InteractableName    = resultElementData.InteractableName;
        elementData.ModelIconPath       = resultElementData.ModelIconPath;
    }

    public void CloseSegment() { }
}
