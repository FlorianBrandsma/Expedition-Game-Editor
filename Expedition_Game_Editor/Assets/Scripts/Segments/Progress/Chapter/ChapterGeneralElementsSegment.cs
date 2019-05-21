using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterGeneralElementsSegment : MonoBehaviour, ISegment
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

    public void InitializeSegment()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;
        
        InitializeElementData();

        SetSearchParameters();
    }

    private void InitializeElementData()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        if (chapterEditor.chapterElementDataList.Count > 0) return;

        ChapterDataElement chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();

        var searchParameters = new Search.ChapterElement();

        searchParameters.requestType = Search.ChapterElement.RequestType.Custom;
        searchParameters.chapterId = new List<int>() { chapterData.id };

        SegmentController.DataController.GetData(new[] { searchParameters });
        
        var chapterElementList = SegmentController.DataController.DataList.Cast<ChapterElementDataElement>().ToList();

        chapterElementList.ForEach(x => chapterEditor.chapterElementDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        ChapterDataElement chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();
        var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Element>().FirstOrDefault();

        //Out of all the elements, select only those that are not in this list
        var idList = SegmentController.DataController.DataList.Cast<ChapterElementDataElement>().Select(x => x.ElementId).ToList();
        var list = dataManager.GetElementData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

        searchParameters.id = list;
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        ChapterDataElement chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
