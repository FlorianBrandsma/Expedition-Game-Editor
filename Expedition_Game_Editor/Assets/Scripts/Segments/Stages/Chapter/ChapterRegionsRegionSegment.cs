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
        var searchParameters = SegmentController.DataController.SearchProperties.searchParameters.Cast<Search.Region>().First();

        searchParameters.excludeId = ChapterEditor.chapterRegionElementDataList.Select(x => x.RegionId).Distinct().ToList();
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

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
