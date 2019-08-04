using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionsRegionSegment : MonoBehaviour, ISegment
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

        if (chapterEditor.chapterRegionDataList.Count > 0) return;

        var chapterData = (ChapterDataElement)DataEditor.Data.dataElement;

        var searchParameters = new Search.ChapterRegion();

        searchParameters.requestType = Search.ChapterRegion.RequestType.Custom;
        searchParameters.chapterId = new List<int>() { chapterData.id };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });

        var chapterRegionList = SegmentController.DataController.DataList.Cast<ChapterRegionDataElement>().ToList();

        chapterRegionList.ForEach(x => chapterEditor.chapterRegionDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        var chapterEditor = (ChapterEditor)DataEditor;

        var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Region>().FirstOrDefault();

        var idList = chapterEditor.chapterRegionDataList.Select(x => x.RegionId).Distinct().ToList();

        var regionList = dataManager.GetRegionData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

        searchParameters.id = regionList;
        searchParameters.phaseId = new List<int>() { 0 };
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
