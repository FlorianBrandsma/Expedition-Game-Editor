using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterElementsPartyElementsSegment : MonoBehaviour, ISegment
{
    private DataManager dataManager = new DataManager();

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    private ElementController ElementController { get { return (ElementController)SegmentController.DataController; } }

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

        if (chapterEditor.partyElementDataList.Count > 0) return;

        var chapterData = (ChapterDataElement)DataEditor.Data.DataElement;

        var searchParameters = new Search.PartyElement();

        searchParameters.requestType = Search.PartyElement.RequestType.Custom;
        searchParameters.chapterId = new List<int>() { chapterData.id };

        SegmentController.DataController.GetData(new[] { searchParameters });

        var partyElementList = SegmentController.DataController.DataList.Cast<PartyElementDataElement>().ToList();
        partyElementList.ForEach(x => chapterEditor.partyElementDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        var chapterData = (ChapterDataElement)DataEditor.Data.DataElement;
        var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Element>().FirstOrDefault();

        List<int> idList = new List<int>();
        chapterEditor.partyElementDataList.ForEach(x => idList.Add(x.ElementId));
        chapterEditor.terrainElementDataList.ForEach(x => idList.Add(x.ElementId));

        var list = dataManager.GetElementData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

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
