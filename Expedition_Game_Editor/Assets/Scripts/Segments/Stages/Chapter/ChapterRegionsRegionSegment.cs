using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionsRegionSegment : MonoBehaviour, ISegment
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

        var searchProperties = new SearchProperties(Enums.DataType.ChapterRegion);

        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterRegion>().First();
        searchParameters.chapterId = new List<int>() { ChapterEditor.ChapterData.Id };

        SegmentController.DataController.DataList = EditorManager.GetData(SegmentController.DataController, searchProperties);

        var chapterRegionList = SegmentController.DataController.DataList.Cast<ChapterRegionDataElement>().ToList();
        chapterRegionList.ForEach(x => ChapterEditor.chapterRegionDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        var searchProperties = SegmentController.DataController.SearchProperties;

        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        var idList = ChapterEditor.chapterRegionDataList.Select(x => x.RegionId).Distinct().ToList();

        //Find interactables where id is not in the list
        var regionList = dataManager.GetRegionData().Where(x => !idList.Contains(x.Id)).Select(x => x.Id).Distinct().ToList();

        searchParameters.id = regionList;
        searchParameters.phaseId = new List<int>() { 0 };
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
