using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterGeneralSupportingCharacterSegment : MonoBehaviour, ISegment
{
    private ChapterEditor ChapterEditor { get { return (ChapterEditor)DataEditor; } }

    private DataManager dataManager = new DataManager();

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.ChapterInteractable);

        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterInteractable>().First();     
        searchParameters.chapterId = new List<int>() { ChapterEditor.ChapterData.Id };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, searchProperties);

        var chapterInteractableList = SegmentController.DataController.DataList.Cast<ChapterInteractableDataElement>().ToList();
        chapterInteractableList.ForEach(x => ChapterEditor.chapterInteractableDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        var searchProperties = SegmentController.DataController.SearchProperties;

        var searchParameters = searchProperties.searchParameters.Cast<Search.Interactable>().First();

        var idList = ChapterEditor.partyMemberDataList.Select(x => x.InteractableId).Union(
                     ChapterEditor.chapterInteractableDataList.Select(x => x.InteractableId)).Distinct().ToList();
        
        //Find interactables where id is not in the list
        var list = dataManager.GetInteractableData().Where(x => !idList.Contains(x.Id)).Select(x => x.Id).Distinct().ToList();

        searchParameters.id = list;
    }

    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
