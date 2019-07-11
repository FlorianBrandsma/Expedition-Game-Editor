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
        DataEditor = SegmentController.editorController.pathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        if (chapterEditor.terrainElementDataList.Count > 0) return;

        var chapterData = (ChapterDataElement)DataEditor.Data.DataElement;

        var searchParameters = new Search.TerrainInteractable();

        searchParameters.requestType = Search.TerrainInteractable.RequestType.Custom;
        searchParameters.chapterId = new List<int>() { chapterData.id };

        SegmentController.DataController.GetData(new[] { searchParameters });
        
        var chapterElementList = SegmentController.DataController.DataList.Cast<TerrainInteractableDataElement>().ToList();
        chapterElementList.ForEach(x => chapterEditor.terrainElementDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        var chapterData = (ChapterDataElement)DataEditor.Data.DataElement;
        var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Interactable>().FirstOrDefault();

        List<int> idList = new List<int>();
        chapterEditor.partyElementDataList.ForEach(x => idList.Add(x.InteractableId));
        chapterEditor.terrainElementDataList.ForEach(x => idList.Add(x.InteractableId));

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
