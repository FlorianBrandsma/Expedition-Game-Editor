using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterGeneralWorldInteractableSegment : MonoBehaviour, ISegment
{
    private DataManager dataManager = new DataManager();

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    private InteractableController ElementController { get { return (InteractableController)SegmentController.DataController; } }

    public void ApplySegment()
    {
        
    }

    public void CloseSegment()
    {
        
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        if (chapterEditor.sceneInteractableDataList.Count > 0) return;

        var chapterData = (ChapterDataElement)DataEditor.Data.dataElement;

        var searchParameters = new Search.SceneInteractable();

        searchParameters.requestType = Search.SceneInteractable.RequestType.Custom;
        searchParameters.chapterId = new List<int>() { chapterData.id };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });
        
        var chapterInteractableList = SegmentController.DataController.DataList.Cast<SceneInteractableDataElement>().ToList();
        chapterInteractableList.ForEach(x => chapterEditor.sceneInteractableDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        var chapterData = (ChapterDataElement)DataEditor.Data.dataElement;
        var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Interactable>().FirstOrDefault();

        List<int> idList = new List<int>();
        chapterEditor.partyMemberDataList.ForEach(x => idList.Add(x.InteractableId));
        chapterEditor.sceneInteractableDataList.ForEach(x => idList.Add(x.InteractableId));

        var list = dataManager.GetInteractableData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

        searchParameters.id = list;
    }

    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
