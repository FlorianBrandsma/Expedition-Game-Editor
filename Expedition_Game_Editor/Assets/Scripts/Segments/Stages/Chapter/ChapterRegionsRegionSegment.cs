using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ChapterRegionsRegionSegment : MonoBehaviour, ISegment
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

        var searchProperties = new SearchProperties(Enums.DataType.ChapterRegion);

        var searchParameters = searchProperties.searchParameters.Cast<Search.ChapterRegion>().First();
        searchParameters.chapterId = new List<int>() { ChapterEditor.Id };

        SegmentController.DataController.GetData(searchProperties);

        var chapterRegionList = SegmentController.DataController.Data.dataList.Cast<ChapterRegionElementData>().ToList();
        chapterRegionList.ForEach(x => ChapterEditor.chapterRegionElementDataList.Add(x));
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

        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        var idList = ChapterEditor.chapterRegionElementDataList.Select(x => x.RegionId).Distinct().ToList();

        //Find interactables where id is not in the list
        var regionList = DataManager.GetRegionData().Where(x => !idList.Contains(x.Id)).Select(x => x.Id).Distinct().ToList();

        searchParameters.id = regionList;
        searchParameters.phaseId = new List<int>() { 0 };
    }

    public void SetSearchResult(IElementData elementData)
    {
        SetSearchChapterRegion((ChapterRegionElementData)elementData);

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }

    private void SetSearchChapterRegion(ChapterRegionElementData resultElementData)
    {
        var elementData = ChapterEditor.chapterRegionElementDataList.Where(x => x.Id == resultElementData.Id).First();

        elementData.RegionId = resultElementData.RegionId;
        elementData.Name = resultElementData.Name;
        elementData.TileIconPath = resultElementData.TileIconPath;
    }

    public void CloseSegment() { }
}
