using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionsRegionsSegment : MonoBehaviour, ISegment
{
    private DataManager dataManager = new DataManager();

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeSegment()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;

        InitializeRegionData();
    }

    private void InitializeRegionData()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        if (chapterEditor.chapterRegionDataList.Count > 0) return;

        ChapterDataElement chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();

        var searchParameters = new Search.ChapterRegion();

        searchParameters.requestType = Search.ChapterRegion.RequestType.Custom;
        searchParameters.chapterId = new List<int>() { chapterData.id };
        searchParameters.temp_id_count = 15;

        SegmentController.DataController.GetData(new[] { searchParameters });

        var chapterRegionList = SegmentController.DataController.DataList.Cast<ChapterRegionDataElement>().ToList();

        chapterRegionList.ForEach(x => chapterEditor.chapterRegionDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        //ChapterDataElement chapterData = DataEditor.Data.ElementData.Cast<ChapterDataElement>().FirstOrDefault();
        //var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Element>().FirstOrDefault();

        //Out of all the elements, select only those that are not in this list
        //var idList = SegmentController.DataController.DataList.Cast<ChapterElementDataElement>().Select(x => x.ElementId).ToList();
        //var list = dataManager.GetElementData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

        //searchParameters.id = list;
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
}
